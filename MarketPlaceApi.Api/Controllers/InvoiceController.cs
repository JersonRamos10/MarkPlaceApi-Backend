using MarketPlaceApi.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using MarketPlaceApi.Business.Exceptions;

namespace MarketPlaceApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [Authorize]
        [HttpGet("{orderId}/pdf")]
        public async Task<IActionResult> GetInvoicePdf(Guid orderId)
        {
        
            var sellerId = GetSellerId(User);

            var pdfBytes = await _invoiceService.GetInvoicePdfAsync(orderId, sellerId);

            return File(pdfBytes, "application/pdf", "factura.pdf");
        }


        private static Guid GetSellerId(ClaimsPrincipal user)
        {
            var sellerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (sellerIdClaim == null)
                    throw new UnauthorizedException("User claim not found");
            var sellerId = Guid.Parse(sellerIdClaim.Value);

            return sellerId;
        }
    }
}