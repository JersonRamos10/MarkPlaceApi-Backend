using System.ComponentModel.DataAnnotations;
using MarketPlaceApi.Business.DTOs.Category;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.DTOs.Products
{
    public sealed record CreateProductRequest(
        [Required(ErrorMessage = "Product name is required")]
        string Name,

        [MaxLength(500)]
        string Description,

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative.")]
        int Stock,

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than 0")]
        decimal  Price,

        bool Warranty,

        [Required(ErrorMessage = "assign at least one category")]
        List<int> CategoriesIds, 

        List<int> LinkIds

    );

}