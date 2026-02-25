using MarketPlaceApi.Business.DTOs.Category;
using MarketPlaceApi.Business.DTOs.Link;
using MarketPlaceApi.Business.DTOs.Seller;

namespace MarketPlaceApi.Business.DTOs.Products
{
    public sealed record ProductResponse (
        Guid ProductId,
        string Name,
        string NumberReference,
        string Description,
        int Stock,
        decimal Price,
        bool Warranty,
        List<CategoryResponse> Categories,
        List<LinkResponse> Links,
        SellerResponse Seller

    );
}
