using MarketPlaceApi.Business.DTOs.Clients;
using MarketPlaceApi.Business.DTOs.Order;
using MarketPlaceApi.Domain.Enums;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest orderRequest);
    Task<IEnumerable<OrderSummaryResponse>> GetOrdersBySellerAsync(Guid sellerId);
    Task<OrderResponse> GetOrderByIdAsync(Guid orderId);
    Task<string> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
}
