using Microsoft.EntityFrameworkCore;
using order_app.entities.Models;
using order_app.entities.Repositories;
using order_app.services.DTOs;

namespace order_app.services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountService _discountService;
        public OrderService(IOrderRepository orderRepository, IDiscountService discountService)
        {
            _orderRepository = orderRepository;
            _discountService = discountService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                CustomerId = request.CustomerId,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            order.Amount = order.Items.Sum(i => i.Price * i.Quantity);
            order.DiscountAmount = await _discountService.CalculateDiscountAsync(order.CustomerId, order.Amount);

            _orderRepository.Add(order);
            await _orderRepository.SaveChangesAsync();
            return order;
        }

        public async Task<OrderAnalytics> GetAnalyticsAsync()
        {
            var orders = _orderRepository.GetAll();
            var fulfilledOrders = await orders.Where(o => o.FulfilledAt.HasValue).ToListAsync();
            var totalOrders = await orders.CountAsync();
            
            var averageOrders = orders.Select(o => o.FinalAmount).ToList().Average();

            var distribution = orders.GroupBy(s => s.Status)
                .Select(g => new KeyValuePair<OrderStatus,int>(g.Key, g.Count())).ToDictionary();

            return new OrderAnalytics(
                AverageOrderValue: orders.Any() ? averageOrders : 0,
                AverageFulfillmentTimeHours: fulfilledOrders.Any()
                    ? fulfilledOrders.Average(o => (o.FulfilledAt!.Value - o.CreatedAt).TotalHours)
                    : 0,
                TotalOrders: totalOrders,
                StatusDistribution: distribution
            );
        }

        public async Task<Order?> GetOrder(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || !IsValidStatusTransition(order.Status, status))
                return null;

            order.Status = status;
            if (status == OrderStatus.Delivered)
                order.FulfilledAt = DateTime.UtcNow;

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
            return order;
        }

        private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
        {
            return (current, next) switch
            {
                (OrderStatus.Pending, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                (_, OrderStatus.Cancelled) when current != OrderStatus.Delivered => true,
                _ => false
            };
        }
    }
}
