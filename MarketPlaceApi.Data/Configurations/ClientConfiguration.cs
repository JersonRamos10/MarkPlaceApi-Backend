using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketPlaceApi.Domain.Entities;
using System.Runtime.CompilerServices;

namespace MarketPlaceApi.Data.Configurations
{
    public class ClientConfiguration: IEntityTypeConfiguration<Client>
    {
        public void Configure (EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(c => c.ClientId);


            builder.HasIndex(c => c.Email).IsUnique();
            builder.HasIndex(c => c.Dui).IsUnique();
            //Propertys

            builder.Property(c => c.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(c => c.LastName).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(150).IsRequired();
            builder.Property(c => c.Dui).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Address).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Phone).HasMaxLength(50).IsRequired();

        }
    }
}