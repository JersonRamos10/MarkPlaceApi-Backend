using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Business.DTOs.User
{
    public sealed record ChangePasswordRequest (
        [Required]
        string CurrentPassword,

        [Required]
        string NewPassword

    );
}
