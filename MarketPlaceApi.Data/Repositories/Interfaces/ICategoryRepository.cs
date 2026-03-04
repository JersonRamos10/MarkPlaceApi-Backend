using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);

        Task <List<Category>> GetAllAsync ();

        Task <Category?> GetByNameAsync (string name);

        Task SaveChangesAsync();
    }
}