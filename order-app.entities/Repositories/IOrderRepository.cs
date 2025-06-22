using order_app.entities.Models;

namespace order_app.entities.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetCustomerOrdersAsync(int customerId);
    }
}
