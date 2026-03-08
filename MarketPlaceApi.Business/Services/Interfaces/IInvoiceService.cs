using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<(Invoice invoice, byte[] pdfBytes)> GenerateAsync(Guid orderId);
        Task<byte[]> GetInvoicePdfAsync(Guid orderId, Guid sellerId);
    }
}