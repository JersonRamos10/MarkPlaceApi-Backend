using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketPlaceApi.Domain.Entities;
using System.Security.Cryptography.X509Certificates;

namespace MarketPlaceApi.Data.Configuration
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            //primary key
            builder.HasKey(o => o.OrderId);

            //properties
            builder.Property(o => o.OrderNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(o => o.PaymentReceiptUrl)
                .HasMaxLength(200);

            builder.Property(o => o.PaymentMethod)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(o => o.OrderDate)
                .HasDefaultValueSql("GETUTCDATE()");

            //Relaciones 1:N con CLIENTE
            builder.HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(oc => oc.ClientId)
                .HasConstraintName("FK_Orders_Clients");;

            //Relacion 1:N con OrderDetail
            builder.HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .HasConstraintName("FK_Orders_OrderDetails");

            //Relacion 1:1 con Invoice
            builder.HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(oi => oi.OrderId)
                .HasConstraintName("FK_Orders_Invoices");
        }
    }
}