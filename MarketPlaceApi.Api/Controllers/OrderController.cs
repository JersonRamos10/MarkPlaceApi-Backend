using System.Security.Claims;
using MarketPlaceApi.Business.DTOs.Order;
using MarketPlaceApi.Business.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService _service ;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrderAsync([FromBody]CreateOrderRequest orderRequest)
        {
            var newOrder = await _service.CreateOrderAsync(orderRequest);
            
            return StatusCode(201, newOrder);
        } 

        [Authorize]
        [HttpGet]

        public async Task<ActionResult<IEnumerable<OrderSummaryResponse>>> GetOrdersAsync(){
            
            var sellerId = GetSellerId(User);
            
            return Ok(await _service.GetOrdersBySellerAsync(sellerId));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetByIdAsync(Guid id){
            return Ok(await _service.GetOrderByIdAsync(id));
        }

        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<string>> UpdateAsync (Guid id, 
        [FromBody] UpdateOrderStatusRequest request){
            
            return Ok (await _service.UpdateOrderStatusAsync(id, request.Status));
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