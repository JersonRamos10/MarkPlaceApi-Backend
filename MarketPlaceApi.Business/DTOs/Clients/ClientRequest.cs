using System.ComponentModel.DataAnnotations;

namespace  MarketPlaceApi.Business.DTOs.Clients
{
    public sealed record ClientRequest (
        [Required]
        [MaxLength(50)]
        string FirstName,

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
        [Phone]
        [MaxLength(50)]
        string Phone

    );
}