using MarketPlaceApi.Business.DTOs.Category;

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateAsync (CreateCategoryRequest categoryRequest);
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
    }
}