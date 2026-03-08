using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Business.Exceptions;

namespace MarketPlaceApi.Business.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repo;

        private readonly IOrderRepository _orderRepo;

        private readonly IPdfService _pdfService;

        public InvoiceService (
        IInvoiceRepository repo, 
        IOrderRepository orderRepo,
        IPdfService pdfService
        )
        {
            _repo = repo;
            _orderRepo = orderRepo;
            _pdfService = pdfService;
        }

        public async Task<(Invoice invoice, byte[] pdfBytes)> GenerateAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new NotFoundException("Order not found");
            
            var existingInvoice = await _repo.GetByOrderIdAsync(orderId);
            if (existingInvoice != null)
            {
                var existingPdf = _pdfService.GenerateInvoicePdf(existingInvoice, order);
                return (existingInvoice, existingPdf);
            }
            var granTotal = order.OrderDetails.Sum(o => o.UnitPriceAtSale * o.Quantity);
            var subTotal = Math.Round(granTotal / 1.13m,2);
            var taxTotal = Math.Round(granTotal - subTotal,2);

            var invoice = new Invoice {
                OrderId = order.OrderId,
                InvoiceNumber = GenerateInvoiceNumber(),
                IssueDate = DateTime.UtcNow,
                GranTotal = granTotal,
                SubTotal =  subTotal,
                TaxTotal = taxTotal
            };

            await _repo.AddAsync(invoice);

            await _repo.SaveChangesAsync();

            var pdfBytes = _pdfService.GenerateInvoicePdf(invoice, order);

            return (invoice, pdfBytes);
        }

        public async Task<byte[]> GetInvoicePdfAsync(Guid orderId, Guid sellerId)
        {
            var invoice = await _repo.GetByOrderIdAsync(orderId) ?? throw new NotFoundException("Invoice not found");
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new NotFoundException("Order not found");

            if (order.SellerId != sellerId)
                throw new ForbiddenException("You are not authorized to access this invoice.");
        

            return _pdfService.GenerateInvoicePdf(invoice, order);
        }

        private string GenerateInvoiceNumber()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string randomPart = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            return $"INV-{datePart}-{randomPart}";
        }
    }
}

