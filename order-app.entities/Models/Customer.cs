namespace order_app.entities.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CustomerSegment Segment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
