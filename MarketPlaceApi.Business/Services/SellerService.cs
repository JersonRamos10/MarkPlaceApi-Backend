using MarketPlaceApi.Business.DTOs.Seller;
using MarketPlaceApi.Business.Exceptions;
using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Data.Repositories.interfaces;
using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Business.Services
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _repo;
        public SellerService (ISellerRepository repo)
        {
            _repo = repo;
        }
        public async Task<SellerProfileResponse> GetProfileAsync(Guid sellerId)
        {
            var seller = await _repo.GetByIdWithUserAsync(sellerId) ?? throw new NotFoundException("Seller not found");

            return MapToDto(seller);
            
        }

        public async Task<SellerProfileResponse> UpdateAsync(Guid sellerId, UpdateSellerRequest request)
        {
            var seller = await _repo.GetByIdForUpdateAsync(sellerId) ?? throw new NotFoundException("Seller not found");


            if(!string.IsNullOrEmpty(request.Name))
            {
                var names = request.Name.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

                seller.FirstName = names.Length > 0 ? names[0] :"";
                seller.LastName  = names.Length > 1 ? names[1]: "";
            }
            

            if (!string.IsNullOrWhiteSpace(request.StoreName))
                seller.StoreName = request.StoreName;

            if (!string.IsNullOrWhiteSpace(request.Phone))
                seller.Phone = request.Phone;

            if (!string.IsNullOrWhiteSpace(request.Direction))
                seller.Direction = request.Direction;

            _repo.Update(seller);

            await _repo.SaveChangesAsync();

            return MapToDto(seller);
        }

        private SellerProfileResponse MapToDto (Seller seller)
        {
            var sellerResponse = new SellerProfileResponse (
                Id: seller.SellerId,
                Name: $"{seller.FirstName} {seller.LastName}",
                Email: seller.User.Email,
                Phone: seller.Phone,
                StoreName: seller.StoreName,
                UserName: seller.User.UserName,
                Direction: seller.Direction
            );

            return sellerResponse;
        }


    }
}