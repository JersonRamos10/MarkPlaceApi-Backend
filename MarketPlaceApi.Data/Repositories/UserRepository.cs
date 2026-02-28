using MarketPlaceApi.Data.Data;
using MarketPlaceApi.Data.Repositories.interfaces;
using MarketPlaceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MarketplaceDbContext _context;

        public UserRepository(MarketplaceDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id) => 
            await _context.Users.FindAsync(id);

        public async Task<User?> GetByEmailAsync(string email) => 
            await _context.Users .AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByUsernameAsync(string username) => 
            await _context.Users .AsNoTracking().FirstOrDefaultAsync(u => u.UserName == username);

        public async Task AddAsync(User user) => 
            await _context.Users.AddAsync(user);

        public async Task<bool> SaveChangesAsync() => 
            (await _context.SaveChangesAsync()) > 0;
    }
}