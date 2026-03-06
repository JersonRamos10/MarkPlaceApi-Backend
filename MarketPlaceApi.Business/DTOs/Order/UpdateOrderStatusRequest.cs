
using System.ComponentModel.DataAnnotations;
using MarketPlaceApi.Domain.Enums;

namespace MarketPlaceApi.Business.DTOs.Order
{
    public sealed record UpdateOrderStatusRequest(
        
    [Required]
    [EnumDataType(typeof(OrderStatus))]
    OrderStatus Status
);
}
