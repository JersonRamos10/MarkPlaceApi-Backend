namespace MarketPlaceApi.Business.DTOs.BankAccount
{
    public sealed record BankAccountResponse(
        int Id,
        string Type,
        string NumberAccount,
        Guid SellerId
    );
}