using Microsoft.AspNetCore.Mvc;
using MarketPlaceApi.Business.Services;
using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MarketPlaceApi.Business.Exceptions;
using MarketPlaceApi.Business.DTOs.Seller;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _service ; 

        public SellerController(ISellerService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<SellerProfileResponse>> GetSellerAsync ()
        {
            var sellerId = GetSellerId(User);
            return Ok(await _service.GetProfileAsync(sellerId));
        }

        [Authorize]
        [HttpPatch]

        public async Task<ActionResult<SellerProfileResponse>> UpdateProfile([FromBody] UpdateSellerRequest updateSeller)
        {
            var sellerProfile = await _service.UpdateAsync(GetSellerId(User), updateSeller);

            return Ok (sellerProfile);
        }

        private static Guid GetSellerId(ClaimsPrincipal user)
        {
            var sellerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (sellerIdClaim == null)
                throw new UnauthorizedException("User claim not found");
            var sellerId = Guid.Parse(sellerIdClaim.Value);

            return sellerId;
        }
    }
}