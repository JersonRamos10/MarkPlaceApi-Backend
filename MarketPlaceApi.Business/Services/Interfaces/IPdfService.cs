using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateInvoicePdf(Invoice invoice, Order order);
    }
}