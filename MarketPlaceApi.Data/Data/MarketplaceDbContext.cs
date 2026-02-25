using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Data
{
    public class MarketplaceDbContext : DbContext
    {
        
        public DbSet<Category> Categories {get; set;}

        public DbSet<Link> Links {get; set;}

        public DbSet<Product> Products {get; set;}

        public DbSet<Client> Clients {get;set;}

        public DbSet<Seller> Sellers {get;set;}

        public DbSet<Order> Orders {get; set;}

        public DbSet<Invoice> Invoices {get;set;}

        public DbSet<User> Users {get;set;}

        public DbSet<OrderDetail> OrderDetails {get;set;}

        public DbSet<BankAccount> BankAccounts {get;set;}
        
        //Constructor
        public MarketplaceDbContext (DbContextOptions<MarketplaceDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarketplaceDbContext).Assembly);
        }
    } 
}