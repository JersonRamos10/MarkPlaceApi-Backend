using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Domain.Entities;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Business.Exceptions;

namespace  MarketPlaceApi.Business.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repo;

        private readonly IOrderRepository _orderRepo;

        public InvoiceService (IInvoiceRepository repo, IOrderRepository orderRepo)
        {
            _repo = repo;
            _orderRepo = orderRepo;
        }
        public async Task<Invoice> GenerateAsync(Guid orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new NotFoundException("Order not found");
            
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

            return invoice;
        }

        private string GenerateInvoiceNumber()
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            string randomPart = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
            return $"INV-{datePart}-{randomPart}";
        }
    }
}