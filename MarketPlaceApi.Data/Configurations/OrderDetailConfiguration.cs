using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketPlaceApi.Domain.Entities;
using System.Security.Cryptography.X509Certificates;

namespace MarketPlaceApi.Data.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.UnitPriceAtSale).HasPrecision(18,2);
            builder.Property(o => o.Quantity).IsRequired();
        }
    }
}