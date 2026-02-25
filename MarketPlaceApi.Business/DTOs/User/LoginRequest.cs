namespace MarketPlaceApi.Business.DTOs.User{
    public record LoginRequest (
        string Username,
        string Password
    );
}