using order_app.entities.Models;
using order_app.services.DTOs;

namespace order_app.services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<OrderAnalytics> GetAnalyticsAsync();
        Task<Order?> GetOrder(int orderId);
    }
}
