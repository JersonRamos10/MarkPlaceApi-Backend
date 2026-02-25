using MarketPlaceApi.Data.Data;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceApi.Data.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly MarketplaceDbContext _context;

        public SellerRepository(MarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Seller seller)
        {
            await _context.Sellers.AddAsync(seller);
        }

        public async Task<Seller?> GetByIdAsync(Guid id)
        {
            return await _context.Sellers.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}