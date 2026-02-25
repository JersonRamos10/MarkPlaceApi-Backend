namespace MarketPlaceApi.Business.DTOs.User
{
    public record UserResponse (
        Guid UserId,
        string UserName,
        string Email
    );
}