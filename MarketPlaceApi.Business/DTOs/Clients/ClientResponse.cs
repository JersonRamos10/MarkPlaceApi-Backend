namespace MarketPlaceApi.Business.DTOs.Clients
{
    public sealed record ClientResponse(
        Guid ClientId,
        string FirtsName,
        string LastName,
        string Dui,
        string Address,
        string Email,
        string Phone
    );
}