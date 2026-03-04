using MarketPlaceApi.Business.DTOs.BankAccount;

namespace MarketPlaceApi.Business.DTOs.Seller
{
    public sealed record SellerProfileResponse(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        string StoreName,
        string UserName,
        string  Direction 
    );
}