using MarketPlaceApi.Business.DTOs.User;
using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
            try 
            {
                var result = await _authService.RegisterAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> LoginAsyinc(LoginRequest request)
        {
            try 
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message); 
            }
        }

        [HttpGet ("{id}")] 

        public async Task<ActionResult<UserResponse>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var result = await _authService.GetUserByIdAsync(id);
                return Ok(result);
            }
            catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize] 
        [HttpPatch("change-password")]
        public async Task<ActionResult<bool>> ChangePassword (ChangePasswordRequest chPass)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null) 
                    return Unauthorized();

                var userId = Guid.Parse(userIdClaim.Value);

                var result = await _authService.ChangePasswordAsync(userId,chPass);
                return Ok(new { message = "Password successfully updated" });
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}