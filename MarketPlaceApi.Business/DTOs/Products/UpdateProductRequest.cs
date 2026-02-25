using System.ComponentModel.DataAnnotations;
using MarketPlaceApi.Business.DTOs.Category;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.DTOs.Products
{
    public sealed record UpdateProductRequest(

        [Required]
        Guid ProductId , 

        string? Name = null,

        [MaxLength(500)]
        string? Description = null,

        
        [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative.")]
        int? Stock = null,

        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than 0")]
        decimal?  Price = null,

        bool? Warranty = null,

        List<int>? CategoryIds = null, 

        List<int>? LinkIds = null

    );

}