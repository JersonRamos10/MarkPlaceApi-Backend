using MarketPlaceApi.Business.DTOs.User;
using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace MarketPlaceApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> RegisterAsync(UserRequest request)
        {
                var result = await _authService.RegisterAsync(request);
                return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> LoginAsyinc(LoginRequest request)
        {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
        }

        [HttpGet ("{id}")] 

        public async Task<ActionResult<UserResponse>> GetUserByIdAsync(Guid id)
        {
                var result = await _authService.GetUserByIdAsync(id);
                return Ok(result);
        }

        [Authorize] 
        [HttpPatch("change-password")]
        public async Task<ActionResult<bool>> ChangePassword (ChangePasswordRequest chPass)
        {
            
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) 
                    throw new UnauthorizedAccessException("User claim not found");

                var userId = Guid.Parse(userIdClaim.Value);

                var result = await _authService.ChangePasswordAsync(userId,chPass);
                return Ok(new { message = "Password successfully updated" });
            
        }
    }
}