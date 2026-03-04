using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceApi.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MarketplaceDbContext _context; 

        public CategoryRepository (MarketplaceDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.IsActive == true)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

            return category;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}