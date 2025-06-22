using order_app.entities.Models;

namespace order_app.services.DTOs
{
    public record OrderAnalytics(
    decimal AverageOrderValue,
    double AverageFulfillmentTimeHours,
    int TotalOrders,
    Dictionary<OrderStatus, int> StatusDistribution
);
}
