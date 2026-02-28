
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Data;
using Microsoft.EntityFrameworkCore;
namespace MarketPlaceApi.Data.Repositories.interfaces
{
    public class ProductRepository : IProductRepository
    {

        private readonly MarketplaceDbContext _context; 

        public ProductRepository (MarketplaceDbContext context)
        {
            _context = context;
        }
        
        public async Task AddAsync(Product product)
        {
            
            await _context.Products.AddAsync(product);
            
        }
        public async Task<Product?> GetByIdWithDetailsAsync(Guid id)
        {
            var product =  await _context.Products .AsNoTracking()
                .Where( p => p.IsActive)
                .Include(p => p.Categories) 
                .Include(p => p.Links)      
                .Include(p => p.Seller)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            return product;
        }

        public async Task<List<Category>> GetCategoriesByIdsAsync(List<int> ids)
        {
            if (ids == null || !ids.Any()) 
                return new List<Category>();
                
            return await _context.Categories
                .Where(c => ids.Contains(c.CategoryId))
                .ToListAsync();
        }

        public async Task<List<Link>> GetLinksByIdsAsync(List<int> ids)
        {
            if (ids == null || !ids.Any()) 
                return new List<Link>();

            return await _context.Links
                .AsNoTracking()
                .Where(c => ids.Contains(c.LinkId))
                .ToListAsync();
        }

        public async Task<(IEnumerable<Product> Items, int Total)> GetPagedAsync(int page, int size, string? name, List<int>? categoryIds)
        {
            var query = _context.Products
                .Where( p => p.IsActive)
                .Include(p => p.Categories)
                .Include(p => p.Links)
                .Include(p => p.Seller)
                .ThenInclude(p => p.User).AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));
    
            if (categoryIds != null && categoryIds.Any())
                query = query.Where(p => p.Categories.Any(c => categoryIds.Contains(c.CategoryId)));
    

            int total = await query.CountAsync();

            var items = await query.OrderBy(p =>p.Name)
                .Skip((page - 1) * size) 
                .Take(size) 
                .ToListAsync(); 

            return (items, total);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}