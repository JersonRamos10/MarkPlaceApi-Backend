using MarketPlaceApi.Business.DTOs.Category;

namespace MarketPlaceApi.Business.DTOs.Pagination
{
    public sealed record FilterRequest (
        string? Name = null,
        List<int>? CategoriesIds = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null    
    );
}