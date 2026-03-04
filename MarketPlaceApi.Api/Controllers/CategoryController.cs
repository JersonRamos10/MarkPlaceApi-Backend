using Microsoft.AspNetCore.Mvc;
using MarketPlaceApi.Business.DTOs.Category;
using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MarketPlaceApi.Domain.Entities;
namespace MarketPlaceApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController:ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController (ICategoryService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpPost]
        public async Task <ActionResult<CategoryResponse>> CreateAsync(CreateCategoryRequest createCategory)
        {
            var result = await _service.CreateAsync(createCategory);

            return Created($"/api/category/{result.Id}", result);
            
        }

        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();

            return Ok (result);
        }
    }
}