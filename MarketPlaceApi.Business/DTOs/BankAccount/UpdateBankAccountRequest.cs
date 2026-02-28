using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Business.DTOs.BankAccount
{
    public sealed record UpdateBankAccountRequest (
        [Required]
        int Id,
        
        [MaxLength(50)]
        string? NumberAccount,

        [MaxLength(50)]
        string? Type
    );
}