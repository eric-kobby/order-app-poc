using order_app.entities.Models;
using System.ComponentModel.DataAnnotations;

namespace order_app.services.DTOs
{
    public record UpdateOrderStatusRequest([Required] OrderStatus Status);
}
