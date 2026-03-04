
using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Business.DTOs.Seller
{
    public sealed record UpdateSellerRequest(
        

        [MaxLength(50)]
        string? Name =  null, 

        [Phone]
        string? Phone = null,

        [MaxLength(100)]
        string? StoreName = null,

        [MaxLength(100)]
        string? Direction = null
    );
}