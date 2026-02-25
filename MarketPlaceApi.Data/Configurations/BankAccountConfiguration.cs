using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Configurations
{
    public class BankAccountConfigurarion : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure (EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("BankAccounts");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.NumberAccount).HasMaxLength(50).IsRequired();
            builder.Property(b => b.Type).HasMaxLength(50).IsRequired();
        }

    
    }
}