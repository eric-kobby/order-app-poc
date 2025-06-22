using Microsoft.EntityFrameworkCore;
using order_app.entities.Models;

namespace order_app.entities.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly OrderAppDbContext _context;

        public OrderRepository(OrderAppDbContext ctx) : base(ctx)
        {
            _context = ctx;
        }
        public async Task<List<Order>> GetCustomerOrdersAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
