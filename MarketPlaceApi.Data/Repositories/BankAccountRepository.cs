using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Data;
using Microsoft.EntityFrameworkCore;


namespace MarketPlaceApi.Data.Repositories
{
    public class BankAccountRepository :IBankAccountRepository
    {
        private readonly MarketplaceDbContext _context;

        public BankAccountRepository (MarketplaceDbContext context)
        {
            _context = context;

        }
        public async Task AddAsync(BankAccount bankAccount)
        {
            await _context.BankAccounts.AddAsync(bankAccount);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id, Guid sellerId)
        {
            var bankAccount = await _context.BankAccounts
                .FirstOrDefaultAsync(b => b.Id == id && b.SellerId == sellerId);

            if(bankAccount == null)
                return false;

            bankAccount.IsActive = false;

            _context.BankAccounts.Update(bankAccount);
            return true; 
        }


        /// <summary>
        /// Obtiene las cuenta bancarias activas 
        /// </summary>
        /// <param name="accountId">El identificador de la cuenta bancaria</param>
        /// <param name="sellerId">El identificador del venedor</param>
        /// <returns>La cuenta bancaria segun id</returns>
        public async Task<BankAccount?> GetAccountByIdAsync(int accountId, Guid sellerId)
        {
            var bankAccount = await _context.BankAccounts.AsNoTracking()    
                    .FirstOrDefaultAsync(b => b.Id == accountId && b.SellerId == sellerId);

            return bankAccount;
        }

        /// <summary>
        ///  Obtiene todas las cuentas bancarias activas de un vendedor. 
        /// </summary>
        /// <param name="sellerId">El identificador único del vendedor.</param>
        /// <returns>Lista de cuentas bancarias activas</returns>
        public async Task<List<BankAccount>> GetAccountsBySellerIdAsync (Guid sellerId)
        {
            var accounts = await _context.BankAccounts
                                .AsNoTracking()
                                .Where(b => b.SellerId == sellerId && b.IsActive == true)
                                .ToListAsync();

            return accounts;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task Update(BankAccount bankAccount)
        {
            _context.BankAccounts.Update(bankAccount);

            await _context.SaveChangesAsync();
        }
    }
}