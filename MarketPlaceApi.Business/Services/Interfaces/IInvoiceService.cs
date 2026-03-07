using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<Invoice> GenerateAsync(Guid orderId);
    }
}