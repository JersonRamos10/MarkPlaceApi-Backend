namespace MarketPlaceApi.Business.DTOs.Category
{
    public sealed record CategoryResponse(
        int Id,
        string Name,
        string? Description
    );
}
