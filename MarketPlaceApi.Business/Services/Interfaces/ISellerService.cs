using MarketPlaceApi.Business.DTOs.Seller;

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface ISellerService
    {
        Task<SellerProfileResponse> GetProfileAsync(Guid sellerId);
        Task<SellerProfileResponse> UpdateAsync(Guid sellerId, UpdateSellerRequest request);
    }
}