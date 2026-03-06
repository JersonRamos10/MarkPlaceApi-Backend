using MarketPlaceApi.Business.DTOs.Clients;
using MarketPlaceApi.Domain.Enums;

namespace MarketPlaceApi.Business.DTOs.Order
{
    public sealed record OrderSummaryResponse(
        Guid OrderId,
        string OrderNumber,
        DateTimeOffset OrderDate,
        OrderStatus OrderStatus,
        PaymentMethod PaymentMethod,
        string PaymentReceiptUrl,
        ClientResponse ClientResponse
    );
}
