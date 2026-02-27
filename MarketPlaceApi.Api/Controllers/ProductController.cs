using Microsoft.AspNetCore.Mvc;
using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using MarketPlaceApi.Business.DTOs.Products;
using MarketPlaceApi.Business.DTOs.Pagination;
using MarketPlaceApi.Business.Exceptions;
using System.Security.Claims;

namespace MarketPlaceApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ProductController : ControllerBase
    {   
        private readonly IProductService _pdtService;
        public ProductController(IProductService pdtService)
        {
            _pdtService = pdtService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> Create([FromBody] CreateProductRequest request)
        {
            var sellerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (sellerIdClaim == null)
                throw new UnauthorizedException("User claim not found");
            
            var sellerId = Guid.Parse(sellerIdClaim.Value);

            var result = await _pdtService.CreateAsync(request, sellerId);
            
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ProductResponse>>> GetAllAsync([FromQuery] PaginedRequest pagined, [FromQuery] FilterRequest filter)
        {
                var result =  await _pdtService.GetAllAsync(pagined, filter);
                return Ok(result);
        }

        [Authorize]
        [HttpPatch ("{id}")]
        public async Task<ActionResult<ProductResponse>> UpdateAsync(Guid id, [FromBody] UpdateProductRequest uptProduct)
        {
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
            if (string.IsNullOrEmpty(claimId))
                throw new UnauthorizedException("User claim not found");

            var sellerId = Guid.Parse(claimId);

            var productUpdate = uptProduct with {ProductId = id} ;

            var result = await _pdtService.UpdateAsync(productUpdate, sellerId);
            return Ok(result);

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var result = await _pdtService.DeleteAsync(id);

            return NoContent();
        }

    }
}