using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Table Name
            builder.ToTable("Products");

            // Primary Key
            builder.HasKey(p => p.ProductId);

            // Propertys
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Price)
                    .HasPrecision(18, 2); 

            builder.Property(p => p.NumberReference)
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(p => p.Stock).IsRequired();
            
            ConfigureRelationships(builder);
        }


        private void ConfigureRelationships(EntityTypeBuilder<Product> builder)
        {
            // Relación 1:N con Seller
            builder.HasOne(p => p.Seller)
                    .WithMany(s => s.Products)
                    .HasForeignKey(p => p.SellerId)
                    .HasConstraintName("FK_Products_Sellers")
                    .OnDelete(DeleteBehavior.Restrict);

            // Relacion 1:N con links
            builder.HasMany(p => p.Links)
                .WithOne(l => l.Product)
                .HasForeignKey(l => l.ProductId)
                .HasConstraintName("FK_Products_Links");;


            //Relacion N:M con categories
            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Products);

            //Relacion 1:N con OrderDetail
            builder.HasMany(p => p.OrderDetails)
                .WithOne(od => od.Product)
                .HasForeignKey(od => od.ProductId)
                .HasConstraintName("FK_Products_OrderDetails");; 
        }
    }
}