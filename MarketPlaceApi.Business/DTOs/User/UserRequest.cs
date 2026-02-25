using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Business.DTOs.User
{
    public record UserRequest (
        [Required]
        [MaxLength(50)]
        string UserName,

        [Required]
        string Email,

        [Required]
        string Password,

        [Required]
        string Phone,    

        string? StoreName,   

        string FirstName,   
        
        string LastName
    );
}