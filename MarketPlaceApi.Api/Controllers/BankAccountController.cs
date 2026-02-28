using MarketPlaceApi.Business.DTOs.BankAccount;
using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MarketPlaceApi.Business.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MarketPlaceApi.Api.Controllers
{
    [ApiController]
    [Route ("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAcService;

        public BankAccountController (IBankAccountService bankAcService)
        {
            _bankAcService = bankAcService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BankAccountResponse>> CreateAsync(CreateBankAccountRequest createRequest)
        {

            var sellerId = GetSellerId(User);

            var bankAccount = await _bankAcService.CreateAsync(createRequest, sellerId);

            return Ok(bankAccount);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BankAccountResponse>> GetByIdAsync(int id)
        {
            var sellerId = GetSellerId(User);

            var bankAccount = await _bankAcService.GetAccountByIdAsync(id, sellerId);
            return Ok(bankAccount);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankAccountResponse>>> GetAllAsync()
        {
            var sellerId = GetSellerId(User);

            var accounts = await _bankAcService.GetAccountsBySellerIdAsync(sellerId);
            return Ok(accounts);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<BankAccountResponse>> UpdateAsync(UpdateBankAccountRequest updateRequest)
        {
            var sellerId = GetSellerId(User);

            var updatedAccount = await _bankAcService.UpdateAsync(sellerId, updateRequest);
            return Ok(updatedAccount);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var sellerId = GetSellerId(User);

            await _bankAcService.DeleteAsync(id, sellerId);
            return NoContent();
        }

        private  Guid GetSellerId(ClaimsPrincipal user)
        {
            var sellerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (sellerIdClaim == null)
                throw new UnauthorizedException("User claim not found");
            var sellerId = Guid.Parse(sellerIdClaim.Value);

            return sellerId;
        }
    }

}