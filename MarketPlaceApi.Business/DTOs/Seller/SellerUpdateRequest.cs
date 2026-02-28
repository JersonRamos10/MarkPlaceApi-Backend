
using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Business.DTOs.Seller
{
    public sealed record UpdateSellerRequest(
        [Required]
        Guid SellerId,

        [MaxLength(50)]
        string? Name =  null, 

        [MaxLength(150)]
        string? Email = null, 

        [MaxLength(100)]
        string? StoreName = null
    );
}