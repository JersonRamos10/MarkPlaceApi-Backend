using MarketPlaceApi.Business.DTOs.Products;

namespace MarketPlaceApi.Business.DTOs.OrderDetail
{
    public sealed record OrderDetailResponse(       
        Guid OrderDetailId,
        int Quantity,
        decimal UnitPriceAtSale,
        ProductResponse Product
    );
}