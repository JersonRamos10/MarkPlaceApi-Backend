namespace MarketPlaceApi.Business.DTOs.Link
{
    public sealed record LinkResponse(
        int Id,
        string Url,
        string Image
    );
}