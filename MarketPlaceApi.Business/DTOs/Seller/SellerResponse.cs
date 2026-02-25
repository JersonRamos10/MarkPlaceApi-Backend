namespace MarketPlaceApi.Business.DTOs.Seller
{
    public sealed record SellerResponse(
        Guid Id,
        string Name,
        string Email,
        string Phone
    );
}