using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Repositories.Interfaces
{
    public interface IInvoiceRepository
    {
        Task AddAsync (Invoice invoice);

        Task SaveChangesAsync();

        Task<Invoice?> GetByOrderIdAsync(Guid orderId);
    }
}