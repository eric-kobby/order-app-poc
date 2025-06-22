namespace order_app.entities.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount => Amount - DiscountAmount;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FulfilledAt { get; set; }
        public List<OrderItem> Items { get; set; } = [];
    }
}
