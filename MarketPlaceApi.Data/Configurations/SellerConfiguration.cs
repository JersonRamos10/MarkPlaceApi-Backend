using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Configurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            //Table Name
            builder.ToTable("Sellers");


            //Primary key
            builder.HasKey(s => s.SellerId);
            builder.Property(s => s.SellerId).ValueGeneratedNever();
            //propertys
            builder.Property(s => s.FirstName).HasMaxLength(100).IsRequired(false);
            builder.Property(s => s.LastName).HasMaxLength(100).IsRequired(false);
            builder.Property(s => s.Direction).HasMaxLength(100);
            builder.Property(s=> s.StoreName).HasMaxLength(100).IsRequired(false);
            builder.Property(s => s.Phone).HasMaxLength(50).IsRequired();

            //Relacion 1:1 con User
            builder.HasOne(s => s.User)
                .WithOne(u => u.Seller)
                .HasForeignKey<Seller>(s => s.UserId)
                .HasConstraintName("FK_Sellers_Users");

                // Relación 1:N con BankAccounts
            builder.HasMany(s => s.BankAccounts)
                .WithOne(b => b.Seller)
                .HasForeignKey(b => b.SellerId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Relacion 1:N con Order
            builder.HasMany(o => o.Orders)
            .WithOne(o => o.Seller)
            .HasForeignKey(b => b.SellerId)
            .HasConstraintName("FK_Sellers_Order")
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
