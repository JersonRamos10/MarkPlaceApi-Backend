using MarketPlaceApi.Business.DTOs.User;
using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Repositories.interfaces;
using MarketPlaceApi.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations;
using MarketPlaceApi.Business.Exceptions;

namespace MarketPlaceApi.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;

        private readonly ISellerRepository _sellerRepo;

        private readonly IConfiguration _config;

        private readonly ITokenService _token;
        public AuthService(IUserRepository userRepo, IConfiguration config, ITokenService token, ISellerRepository sellerRepo)
        {
            _userRepo = userRepo;
            _config = config;
            _token = token;
            _sellerRepo = sellerRepo;
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid userId)
        {
            var userFound = await _userRepo.GetByIdAsync(userId);

            if(userFound == null)
                throw new NotFoundException("User not found"); 

            var result = new UserResponse(
                userFound.UserId,
                userFound.UserName,
                userFound.Email

            ); 

            return result; 
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest login)
        {
            var user = await _userRepo.GetByUsernameAsync(login.Username);

            if(user == null)
                throw new ValidationException("Invalid username or password");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

            if (!isPasswordValid)
                throw new ValidationException("Invalid username or password");

            var token = _token.CreateToken(user);

            return new AuthResponse (
                Token: token,
                Expiration: DateTime.UtcNow.AddDays(7),
                User: new UserResponse (user.UserId, user.UserName, user.Email)
            );


        }

        public async Task<UserResponse> RegisterAsync(UserRequest user)
        {
            //logic to register user
            

            var EmailDuplicate = await _userRepo.GetByEmailAsync(user.Email);

            if (EmailDuplicate != null)
                throw new ValidationException("Email already exists");

            var UsernameDuplicate = await _userRepo.GetByUsernameAsync(user.UserName);
            if (UsernameDuplicate != null)
                throw new ValidationException("Username already exists");

            if (!IsPasswordSecure(user.Password))
                throw new ValidationException("The password must be at least 8 characters long, including uppercase letters, lowercase letters, numbers, and one special character.");


            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash the password using BCrypt


            Guid sharedId = Guid.NewGuid();
            // created the instance of the real Entity
                var newUser = new User
                {
                    UserId = sharedId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Password = passwordHash 
            
                };
                var newSeller = new Seller
                {
                    SellerId = sharedId,
                    User = newUser,
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                    Phone = user.Phone,
                    StoreName = user.StoreName ?? $"{user.UserName}'s Shop",
                    IsActive = true

                };
                
                await _userRepo.AddAsync(newUser);
                await _sellerRepo.AddAsync(newSeller);
                await _userRepo.SaveChangesAsync();

                // Convert the User entity to UserResponse DTO
                return new UserResponse(
                    newUser.UserId,
                    newUser.UserName,
                    newUser.Email
                );


        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
        {
            // Search for the user by the ID that comes from the Token
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null) 
                throw new NotFoundException("User not found");

            // verified that the current password is correct
            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password))
                throw new ValidationException("The current password is incorrect.");
            

            if (!IsPasswordSecure(request.NewPassword)) //validated the security of the NEW password
                throw new ValidationException("The new password does not meet the requirements");
            

            // 4. Hash new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            
            return await _userRepo.SaveChangesAsync();
        }
        private bool IsPasswordSecure(string password)
        {
            if(string.IsNullOrWhiteSpace(password) || password.Length < 8)

                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasNumber = password.Any(char.IsDigit);

            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasNumber && hasSpecial;
        }
    } 
}

