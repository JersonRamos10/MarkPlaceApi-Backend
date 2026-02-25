using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}