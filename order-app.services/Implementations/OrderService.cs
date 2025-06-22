using order_app.entities.Models;
using order_app.services.DTOs;

namespace order_app.services.Implementations
{
    public class OrderService : IOrderService
    {
        public Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OrderAnalytics> GetAnalyticsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
