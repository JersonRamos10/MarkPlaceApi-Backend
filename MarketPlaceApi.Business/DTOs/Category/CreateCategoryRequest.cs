using System.ComponentModel.DataAnnotations;

namespace MarketPlaceApi.Business.DTOs.Category
{
    public sealed record CreateCategoryRequest(
        [Required]
        string Name,
        
        string? Description

        
    );
}
