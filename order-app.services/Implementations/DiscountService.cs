
using order_app.entities.Models;
using order_app.entities.Repositories;

namespace order_app.services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public DiscountService(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
        }

        public async Task<decimal> CalculateDiscountAsync(int customerId, decimal orderAmount)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) return 0;

            var orderHistory = await _orderRepository.GetCustomerOrdersAsync(customerId);
            var orderCount = orderHistory.Count;

            var segmentDiscount = GetSegmentDiscount(customer.Segment);
            var loyaltyDiscount = GetLoyaltyDiscount(orderCount);
            var volumeDiscount = GetVolumeDiscount(orderAmount);

            var totalDiscountRate = Math.Min(segmentDiscount + loyaltyDiscount + volumeDiscount, 0.3m); // Max 30%
            return orderAmount * totalDiscountRate;
        }

        private static decimal GetSegmentDiscount(CustomerSegment segment) => segment switch
        {
            CustomerSegment.Premium => 0.05m,
            CustomerSegment.VIP => 0.10m,
            _ => 0m
        };

        private static decimal GetLoyaltyDiscount(int orderCount) => orderCount switch
        {
            >= 10 => 0.10m,
            >= 5 => 0.05m,
            _ => 0m
        };

        private static decimal GetVolumeDiscount(decimal amount) => amount switch
        {
            >= 1000 => 0.15m,
            >= 500 => 0.10m,
            >= 200 => 0.05m,
            _ => 0m
        };
    }
}
