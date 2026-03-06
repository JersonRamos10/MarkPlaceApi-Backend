namespace MarketPlaceApi.Business.DTOs.Clients
{
    public sealed record ClientResponse(
        Guid ClientId,
        string Name,
        string Address,
        string Email,
        string Phone
    );
}