
using System.Linq.Expressions;
using MarketPlaceApi.Business.DTOs.BankAccount;
using MarketPlaceApi.Business.Exceptions;
using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;


namespace MarketPlaceApi.Business.Services
{
    /// <summary>
    /// Servicio para la gestión de cuentas bancarias de vendedores.
    /// </summary>
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _repo;

        public BankAccountService(IBankAccountRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Crea una nueva cuenta bancaria para un vendedor.
        /// </summary>
        /// <param name="createRequest">Datos para la creación de la cuenta bancaria.</param>
        /// <returns>La cuenta bancaria creada en formato de respuesta.</returns>
        public async Task<BankAccountResponse> CreateAsync(CreateBankAccountRequest createRequest, Guid sellerId)
        {
            if(string.IsNullOrEmpty(createRequest.NumberAccount))
                throw new BusinessValidationException("The account number is required");

            if(string.IsNullOrEmpty(createRequest.Type))
                throw new BusinessValidationException("Type account is required");

            // mapping to entity 

            var newAccount = new BankAccount
            {
                NumberAccount = createRequest.NumberAccount,
                Type = createRequest.Type,
                SellerId = sellerId,
                IsActive = true
            };

            await _repo.AddAsync(newAccount);
            await _repo.SaveChangesAsync();

            return MapToDto(newAccount); //map entity to dto
        }

        /// <summary>
        /// Elimina lógicamente una cuenta bancaria de un vendedor.
        /// </summary>
        /// <param name="id">Id de la cuenta bancaria.</param>
        /// <param name="sellerId">Id del vendedor.</param>
        /// <returns>True si la eliminación fue exitosa.</returns>
        public async Task<bool> DeleteAsync(int id, Guid sellerId)
        {
            var result = await _repo.Delete(id,sellerId);

            if (!result)
                throw new NotFoundException("Account not found");
            
            await _repo.SaveChangesAsync();

            return true;
        }
        
        /// <summary>
        /// Obtiene una cuenta bancaria específica de un vendedor por su id.
        /// </summary>
        /// <param name="accountId">Id de la cuenta bancaria.</param>
        /// <param name="sellerId">Id del vendedor.</param>
        /// <returns>La cuenta bancaria encontrada en formato de respuesta.</returns>
        public async Task<BankAccountResponse> GetAccountByIdAsync(int accountId, Guid sellerId)
        {
            var bankAccount = await _repo.GetAccountByIdAsync(accountId, sellerId);

            if(bankAccount == null)
                throw new NotFoundException(" Account not found");

            //mapping to bankAccountResponse 
            return MapToDto(bankAccount);
        }

        /// <summary>
        /// Obtiene todas las cuentas bancarias activas de un vendedor.
        /// </summary>
        /// <param name="sellerId">Id del vendedor.</param>
        /// <returns>Lista de cuentas bancarias en formato de respuesta.</returns>
        public async Task<IEnumerable<BankAccountResponse>> GetAccountsBySellerIdAsync (Guid sellerId)
        {
            var bankAccount = await _repo.GetAccountsBySellerIdAsync(sellerId);

            if(bankAccount == null || bankAccount.Count ==  0)
                return new List<BankAccountResponse> ();

            //mapping
            return bankAccount.Select(MapToDto);
            
        }

        /// <summary>
        /// Actualiza los datos de una cuenta bancaria de un vendedor.
        /// </summary>
        /// <param name="sellerId">Id del vendedor.</param>
        /// <param name="updateRequest">Datos para actualizar la cuenta bancaria.</param>
        /// <returns>La cuenta bancaria actualizada en formato de respuesta.</returns>
        public async Task<BankAccountResponse> UpdateAsync(Guid sellerId, UpdateBankAccountRequest updateRequest)
        {
            var accountExist = await _repo.GetAccountByIdAsync(updateRequest.Id, sellerId);

            if(accountExist == null) 
                throw new NotFoundException ("Account not found");

            if (!string.IsNullOrEmpty(updateRequest.NumberAccount))
            {
                accountExist.NumberAccount = updateRequest.NumberAccount;
            }

            if (!string.IsNullOrEmpty(updateRequest.Type))
            {
                accountExist.Type = updateRequest.Type;
            }

            await _repo.Update(accountExist);
                
            return MapToDto(accountExist);    

        }

        //metodo privado para mappeo de entidad a dto
        private BankAccountResponse MapToDto (BankAccount bankAccount)
        {
            //entity to dto

            var accountResponse = new BankAccountResponse (
                Id : bankAccount.Id,
                Type: bankAccount.Type,
                NumberAccount: bankAccount.NumberAccount,
                bankAccount.SellerId
            );

            return accountResponse; 

        }
    }
}