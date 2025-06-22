using System.ComponentModel.DataAnnotations;

namespace order_app.services.DTOs
{
    public record CreateOrderRequest(
        [Required] int CustomerId,
        [Required] List<OrderItemRequest> Items
    );
}
