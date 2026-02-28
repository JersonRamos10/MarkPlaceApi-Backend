using MarketPlaceApi.Domain.Entities;

namespace MarketPlaceApi.Data.Repositories.Interfaces
{
    public interface IBankAccountRepository
    {
        Task AddAsync (BankAccount bankAccount);

        Task<bool> Delete(int id, Guid sellerId);

        public Task Update(BankAccount bankAccount);

        Task<bool> SaveChangesAsync();

        Task<List<BankAccount>> GetAccountsBySellerIdAsync (Guid sellerId);

        Task<BankAccount?> GetAccountByIdAsync (int accountId, Guid sellerId);
    }
}