using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using order_app.entities.Models;
using order_app.services.DTOs;
using order_app.services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        /// <summary>
        /// Creates a new order with automatic discount calculation
        /// </summary>
        /// <remarks>
        /// Creates a new order with automatic discount calculation based on customer segment and history
        /// </remarks>
        [HttpPost]
        [ProducesResponseType<Order>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        /// <summary>
        /// Updates order status with validation
        /// </summary>
        [HttpPatch("{id}/status")]
        [ProducesResponseType<Order>(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, request.Status);

            if (updatedOrder == null)
                return NotFound("Order not found or invalid status transition");

            return Ok(updatedOrder);
        }

        /// <summary>
        /// Retrieves comprehensive order analytics
        /// </summary>
        /// <remarks>
        /// Returns comprehensive analytics including average values, fulfillment times, and status distribution
        /// </remarks>
        [HttpGet("analytics")]
        [SwaggerResponse(200, "Analytics retrieved successfully", typeof(OrderAnalytics))]
        public async Task<ActionResult<OrderAnalytics>> GetAnalytics()
        {
            var analytics = await _orderService.GetAnalyticsAsync();
            return Ok(analytics);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Order", Description = "Retrieves a specific order by ID")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            // Implementation for GetOrder endpoint
            return Ok(new Order { Id = id });
        }
    }
}
