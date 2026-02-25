namespace MarketPlaceApi.Business.DTOs.User
{
    public record AuthResponse (
        string Token,
        DateTime Expiration,
        UserResponse User
    );
}