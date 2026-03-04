using MarketPlaceApi.Data.Data;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;


namespace MarketPlaceApi.Data.Repositories
{
    public class SellerRepository:ISellerRepository
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

        public async Task<Seller?> GetByIdAsync(Guid sellerId)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.SellerId == sellerId);

            return seller;
        }

        public async Task<Seller?> GetByIdForUpdateAsync(Guid sellerId)
        {
            var seller = await _context.Sellers 
                        .Where(s => s.IsActive == true)
                        .Include(s => s.User)
                        .FirstOrDefaultAsync(s => s.SellerId == sellerId);

            return seller;
        }

        public async Task<Seller?> GetByIdWithUserAsync(Guid sellerId)
        {
            var seller = await _context.Sellers 
                        .AsNoTracking()
                        .Where(s => s.IsActive == true)
                        .Include(s => s.User)
                        .FirstOrDefaultAsync(s => s.SellerId == sellerId);

            return seller;
        }
        public void Update(Seller seller)
        {
            _context.Sellers.Update(seller);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}