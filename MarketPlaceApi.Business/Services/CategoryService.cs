using System.Linq.Expressions;
using MarketPlaceApi.Business.DTOs.Category;
using MarketPlaceApi.Business.Exceptions;
using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Data.Migrations;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService (ICategoryRepository repo)
        {
            _repo = repo;
        }
        public async Task<CategoryResponse> CreateAsync (CreateCategoryRequest categoryRequest)
        {
            if(string.IsNullOrEmpty(categoryRequest.Name))
                throw new BusinessValidationException("Category name cannot be empty");
            

            var categoryExist = await _repo.GetByNameAsync(categoryRequest.Name);

            if(categoryExist != null)
                throw new ConflictException($"A category with the name '{categoryRequest.Name}' already exists");

            //mapping to entity 

            var category = new Category
            {
                Name = categoryRequest.Name,
                Description = categoryRequest.Description,
                IsActive = true,
            };
            
            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();

            return  MapToDto(category);
        }
        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            
            if(categories == null || categories.Count == 0)
            return new List<CategoryResponse>();

            return categories.Select(MapToDto).ToList();

        }

        private CategoryResponse MapToDto (Category category)
        {
            var categoryResponse = new CategoryResponse (
                Id: category.CategoryId,
                Name: category.Name,
                Description: category.Description,
                isActive: category.IsActive
            );

            return categoryResponse;

        }
    }
}