using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Repositories.Interfaces
{
    public interface ISellerRepository
    {
        Task AddAsync(Seller seller);
        Task<Seller?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
    }
}