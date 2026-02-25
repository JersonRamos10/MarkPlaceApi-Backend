using MarketPlaceApi.Business.DTOs.Products;
using MarketPlaceApi.Business.DTOs.Pagination;

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> CreateAsync (CreateProductRequest productRequest, Guid sellerId);

        Task<ProductResponse> UpdateAsync (UpdateProductRequest updateRequest, Guid SellerId);

        Task<PagedResponse<ProductResponse>> GetAllAsync(PaginedRequest pagined, FilterRequest filter);
        Task<ProductResponse> GetByIdAsync (Guid Id);
        Task<bool> DeleteAsync (Guid Id); 
    }
}