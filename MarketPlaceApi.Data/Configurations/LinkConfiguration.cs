using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Configurations
{
    public class linkConfiguration : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            //tableName
            builder.ToTable("Links");

            //Primary key
            builder.HasKey (l => l.LinkId);


            //propertys 
            builder.Property (l => l.Url).HasMaxLength(200);
            builder.Property(l => l.Image).HasMaxLength(200).IsRequired();

        builder.HasData(
        new Link { LinkId = 1, Url = "https://misitio.com/imagen1.jpg", IsActive = true, ProductId = null}
        );

            
        }
    }
}