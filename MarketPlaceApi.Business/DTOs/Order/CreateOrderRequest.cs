using System.ComponentModel.DataAnnotations;
using MarketPlaceApi.Business.DTOs.Clients;
using MarketPlaceApi.Domain.Enums;

namespace MarketPlaceApi.Business.DTOs.Order
{
    
    public sealed record CreateOrderRequest(
        [Required]
        ClientRequest Client,

        [Required]
        Guid ProductId,

        [EnumDataType(typeof(PaymentMethod))]
        PaymentMethod PaymentMethod,

        [Required]
        [MaxLength(200)]
        [Url]
        string PaymentReceiptUrl,

        [Required]
        [Range(1, 100)]
        int Quantity
    );
}