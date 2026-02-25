using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MarketPlaceApi.Data.Data
{
    public class MarketplaceDbContextFactory : IDesignTimeDbContextFactory<MarketplaceDbContext>
    {
        public MarketplaceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MarketplaceDbContext>();
            
            // Usa la misma cadena de conexión que tienes en tu appsettings.json
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=MarketPlaceDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new MarketplaceDbContext(optionsBuilder.Options);
        }
    }
}