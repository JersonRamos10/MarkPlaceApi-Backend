using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure (EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.InvoiceNumber).HasMaxLength(50).IsRequired();
            builder.Property(i => i.GranTotal).HasPrecision(18,2);
            builder.Property(i => i.TaxTotal).HasPrecision(18,2);
        }
    }
}