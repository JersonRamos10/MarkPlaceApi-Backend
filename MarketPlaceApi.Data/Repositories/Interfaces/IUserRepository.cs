using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Repositories.interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}