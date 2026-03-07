using MarketPlaceApi.Data.Data;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceApi.Data.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {   
        private readonly MarketplaceDbContext _context;

        public InvoiceRepository(MarketplaceDbContext context)
        {
            _context=context;
        }
        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }

        public async Task<Invoice?> GetByOrderIdAsync(Guid orderId)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.OrderId == orderId);

            return invoice;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}