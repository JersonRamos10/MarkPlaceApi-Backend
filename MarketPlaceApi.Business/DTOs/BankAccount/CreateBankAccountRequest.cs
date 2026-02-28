using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Business.DTOs.BankAccount
{
    public sealed record CreateBankAccountRequest (
        [Required]
        [MaxLength(50)]
        string NumberAccount,

        [Required]
        [MaxLength(50)]
        string Type
    );
}