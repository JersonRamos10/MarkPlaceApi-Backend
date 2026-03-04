using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Repositories.interfaces
{
    public interface ISellerRepository
    {
        Task  AddAsync (Seller seller);

        Task <Seller?> GetByIdAsync (Guid sellerId);

        Task<Seller?> GetByIdWithUserAsync(Guid sellerId);

        Task<Seller?> GetByIdForUpdateAsync (Guid sellerId);

        void Update(Seller seller);

        Task<bool> SaveChangesAsync();


    }    
}
