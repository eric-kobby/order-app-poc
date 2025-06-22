using System.ComponentModel.DataAnnotations;

namespace order_app.services.DTOs
{
    public record OrderItemRequest(
    [Required] string ProductName,
    [Required, Range(1, int.MaxValue)] int Quantity,
    [Required, Range(0.01, double.MaxValue)] decimal Price
);
}
