using System.ComponentModel.Design.Serialization;
using MarketPlaceApi.Domain.Entities; 

namespace MarketPlaceApi.Data.Repositories.interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdWithDetailsAsync(Guid id);
        Task<List<Category>> GetCategoriesByIdsAsync(List<int> ids);

        Task<List<Link>> GetLinksByIdsAsync(List<int> ids);
        
        Task<(IEnumerable<Product> Items, int Total)> GetPagedAsync(
            int page, 
            int size, 
            string? name, 
            List<int>? categoryIds
        );

        Task AddAsync(Product product);
        void Update(Product product); 

        Task<bool> SaveChangesAsync();

    }
}