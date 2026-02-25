using MarketPlaceApi.Business.DTOs.User;

namespace MarketPlaceApi.Business.Services.Interfaces 
{
    public interface IAuthService 
    {
        // Define methods for authentication and authorization

        //method for user registration
        Task<UserResponse> RegisterAsync(UserRequest user);
        
        //method for GET user by id
        Task<UserResponse> GetUserByIdAsync(Guid userId);

        //method for user login
        Task<AuthResponse> LoginAsync(LoginRequest login);

        //method for changepassword of user
        public Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    }
}