using MarketPlaceApi.Domain.Entities;
namespace MarketPlaceApi.Data.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync (Order order);

        Task SaveChangesAsync();

        Task<Order?> GetByIdAsync (Guid id);

        Task<IEnumerable<Order>> GetBySellerIdAsync(Guid sellerId);

        void Update (Order order);

    }
}