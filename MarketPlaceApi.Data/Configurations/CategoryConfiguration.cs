using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure (EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(100);

            builder.HasData(
            new Category { CategoryId = 1, Name = "Electrónica", IsActive = true },
            new Category { CategoryId = 2, Name = "Hogar y Cocina", IsActive = true },
            new Category { CategoryId = 3, Name = "Moda", IsActive = true }
        );
        }
    }
}