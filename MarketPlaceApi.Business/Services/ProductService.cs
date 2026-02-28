using MarketPlaceApi.Business.DTOs.Pagination;
using MarketPlaceApi.Business.DTOs.Products;
using MarketPlaceApi.Business.DTOs.Category;
using MarketPlaceApi.Business.DTOs.Link;
using MarketPlaceApi.Business.DTOs.Seller;
using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Data.Repositories.interfaces;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Business.Exceptions;



namespace MarketPlaceApi.Business.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _repo;

        public  ProductService (IProductRepository repo)
        {
            _repo = repo;
        }
        public async Task<ProductResponse> CreateAsync(CreateProductRequest productRequest, Guid sellerId)
        {    
        
            if(string.IsNullOrWhiteSpace(productRequest.Name))
                throw new BusinessValidationException("The product name cannot be empty", "Name");

            if(productRequest.Stock < 0)
                throw new BusinessValidationException("The value does not conform to the correct format; a positive numeric value was expected.", "Stock");
            
            var newProduct = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = productRequest.Name,
                Description = productRequest.Description ?? string.Empty, 
                Stock = productRequest.Stock,
                Price = productRequest.Price,
                Warranty = productRequest.Warranty,
                NumberReference = GenerateNumberReference(),
                SellerId = sellerId,
                IsActive = true 
            };

            await _repo.AddAsync(newProduct);
            await _repo.SaveChangesAsync();

            // Manejo de Categorías
            if (productRequest.CategoriesIds != null && productRequest.CategoriesIds.Any())
            {
                var categories = await _repo.GetCategoriesByIdsAsync(productRequest.CategoriesIds);
                if (categories.Any())
                {
                    newProduct.Categories = categories;
                }
            }

            if (productRequest.LinkIds != null && productRequest.LinkIds.Any())
            {
                    var links = await _repo.GetLinksByIdsAsync(productRequest.LinkIds);
                    if (links.Any())
                    {
                        foreach (var link in links)
                        {
                            link.ProductId = newProduct.ProductId;
                        }
                        newProduct.Links = links;
                    }
            }
            await _repo.SaveChangesAsync();

            return await GetByIdAsync(newProduct.ProductId);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _repo.GetByIdWithDetailsAsync(id) ?? throw new NotFoundException("Product don't exist");
            product.IsActive = false; 
            
            return  await _repo.SaveChangesAsync();
        }

        public async Task<PagedResponse<ProductResponse>> GetAllAsync(PaginedRequest pagined, FilterRequest filter)
        {
            var (products, totalItems)  = await _repo.GetPagedAsync(
                pagined.PageNumber, 
                pagined.PageSize, 
                filter.Name, 
                filter.CategoriesIds
            );

            int totalPages = (int)Math.Ceiling((double)totalItems / pagined.PageSize);

            var product = products.Select(MapToResponse).ToList();

            return new PagedResponse<ProductResponse>(
            product,
            totalItems,
            pagined.PageNumber,
            pagined.PageSize,
            totalPages
        );
        }

        public async Task<ProductResponse> GetByIdAsync(Guid id)
        {
            var product = await _repo.GetByIdWithDetailsAsync(id) ?? throw new NotFoundException("Requested product not found");
            return MapToResponse(product);
        }

        public async Task<ProductResponse> UpdateAsync(UpdateProductRequest updateRequest, Guid sellerId)
        {   
            var productExist = await _repo.GetByIdWithDetailsAsync(updateRequest.ProductId);

            if(productExist == null)
                throw new NotFoundException("Product not found");

            if (productExist.SellerId != sellerId)
                throw new ForbiddenException("You don't have permission to update this product");

            if (!string.IsNullOrWhiteSpace(updateRequest.Name))
            {
                productExist.Name = updateRequest.Name;
            }

            if (updateRequest.Description != null)
            {
                productExist.Description = updateRequest.Description;
            }
            

            if (updateRequest.Price.HasValue)
            {
                productExist.Price = updateRequest.Price.Value;
            }

            if (updateRequest.Stock.HasValue)
            {
                productExist.Stock = updateRequest.Stock.Value;
            }

            if (updateRequest.Warranty.HasValue)
            {
                productExist.Warranty = updateRequest.Warranty.Value;
            }

            if (updateRequest.CategoryIds != null && updateRequest.CategoryIds.Any())
            {
                var newCategories = await _repo.GetCategoriesByIdsAsync(updateRequest.CategoryIds);
                    productExist.Categories = newCategories;
            }

            await _repo.SaveChangesAsync();

            return MapToResponse(productExist);

        }

        private string GenerateNumberReference()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string randomPart = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            return $"PRD-{datePart}-{randomPart}";
        }
        private ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse(
                product.ProductId,
                product.Name,
                product.NumberReference,
                product.Description,
                product.Stock,
                product.Price,
                product.Warranty,
                
                product.Categories.Select(c => new CategoryResponse(c.CategoryId, c.Name, c.Description)).ToList(),
                product.Links.Select(l => new LinkResponse(l.LinkId, l.Url, l.Image)).ToList(),
                new SellerResponse(
                    product.Seller.UserId, 
                    product.Seller.User.UserName, 
                    product.Seller.User.Email,
                    product.Seller.Phone,
                    product.Seller.StoreName    
                )
            );
        }
    }


}