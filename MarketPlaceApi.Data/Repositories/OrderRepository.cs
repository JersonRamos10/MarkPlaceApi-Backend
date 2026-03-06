using MarketPlaceApi.Data.Data;
using MarketPlaceApi.Data.Repositories.Interfaces;
using MarketPlaceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceApi.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MarketplaceDbContext _context;

        public OrderRepository(MarketplaceDbContext context)=>
            _context = context;

        public async Task AddAsync(Order order)=>
            await _context.Orders.AddAsync(order);


        public async Task<Order?> GetByIdAsync(Guid id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .AsSplitQuery()
                .Include(o => o.Client)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            return order; 
        }

        public async Task<IEnumerable<Order>> GetBySellerIdAsync(Guid sellerId)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Client) 
                .Where(o => o.SellerId == sellerId)
                .ToListAsync();

            return orders;
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
        

        public void Update(Order order) =>
            _context.Orders.Update(order);

    }
}