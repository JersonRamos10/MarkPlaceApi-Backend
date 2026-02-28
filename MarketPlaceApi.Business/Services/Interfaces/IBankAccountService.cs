using MarketPlaceApi.Business.DTOs.BankAccount; 

namespace MarketPlaceApi.Business.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task<BankAccountResponse> CreateAsync(CreateBankAccountRequest createRequest, Guid sellerId);

        Task<BankAccountResponse> UpdateAsync(Guid sellerId, UpdateBankAccountRequest updateRequest);

        Task<bool> DeleteAsync(int id, Guid sellerId);

        Task<IEnumerable<BankAccountResponse>> GetAccountsBySellerIdAsync (Guid sellerId);

        Task<BankAccountResponse> GetAccountByIdAsync(int accountId, Guid SellerId); 
    }
}