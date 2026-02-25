namespace MarketPlaceApi.Business.DTOs.Pagination
{
    public sealed record PaginedRequest(
    int PageSize,
    int PageNumber
    );
}