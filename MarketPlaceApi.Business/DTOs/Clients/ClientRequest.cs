using System.ComponentModel.DataAnnotations;

namespace  MarketPlaceApi.Business.DTOs.Clients
{
    public sealed record ClientRequest (
        [Required]
        [MaxLength(50)]
        string FirtsName,

        [Required]
        [MaxLength(50)]
        string LastName,

        [Required]
        [MaxLength(50)]
        string Dui,

        [Required]
        [MaxLength(50)]
        string Address,

        [Required]
        [MaxLength(150)]
        string Email,

        [Required]
        [MaxLength(50)]
        string Phone

    );
}