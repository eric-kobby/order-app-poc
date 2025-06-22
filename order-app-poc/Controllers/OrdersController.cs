using Microsoft.AspNetCore.Mvc;
using order_app.entities.Models;
using order_app.services.DTOs;
using order_app.services;
using Swashbuckle.AspNetCore.Annotations;

namespace order_app_poc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        [HttpPost]
        [SwaggerOperation(Summary = "Create Order", Description = "Creates a new order with automatic discount calculation based on customer segment and history")]
        [SwaggerResponse(StatusCodes.Status201Created, "Order created successfully", typeof(Order))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        
        [HttpPatch("{id:int}/status")]
        [SwaggerOperation(Summary = "Update Order Status", Description = "Updates order status with proper state transition validation")]
        [SwaggerResponse(StatusCodes.Status200OK, "Status updated successfully", typeof(Order))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, request.Status);

            if (updatedOrder == null)
                return BadRequest("Order not found or status update failed");

            return Ok(updatedOrder);
        }
        
        [HttpGet("analytics")]
        [SwaggerOperation(Summary = "Get Order Analytics", Description = "Returns comprehensive analytics including average values, fulfillment times, and status distribution")]
        [SwaggerResponse(StatusCodes.Status200OK, "Analytics retrieved successfully", typeof(OrderAnalytics))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error")]
        public async Task<ActionResult<OrderAnalytics>> GetAnalytics()
        {
            var analytics = await _orderService.GetAnalyticsAsync();
            return Ok(analytics);
        }
        
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get Order", Description = "Retrieves a specific order by ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order retrieved successfully", typeof(Order))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null) return NotFound("Order not found");
            return Ok(order);
        }
    }
}
