using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using order_app.entities;
using order_app.entities.Models;
using order_app.services.DTOs;

namespace order_app.tests.Integration_tests;

public class OrderControllerTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestCase]
    public async Task CreateOrder_ValidRequest_ReturnsCreatedOrder()
    {
        // Arrange
        var request = new CreateOrderRequest(
            CustomerId: 2, // Premium customer
            Items:
            [
                new OrderItemRequest("Product A", 2, 50m),
                new OrderItemRequest("Product B", 1, 100m)
            ]
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var order = JsonSerializer.Deserialize<Order>(responseContent, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        Assert.That(order, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(order.Amount, Is.EqualTo(200m));
            Assert.That(order.DiscountAmount, Is.GreaterThan(0)); // Premium customer should get discount
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Pending));
        });
    }

    [TestCase]
    public async Task GetAnalytics_ReturnsValidAnalytics()
    {
        // Arrange - Create some test orders first
        var createRequest = new CreateOrderRequest(
            CustomerId: 1,
            Items: [new OrderItemRequest("Test Product", 1, 50m)]
        );
        await _client.PostAsJsonAsync("/api/orders", createRequest);

        // Act
        var response = await _client.GetAsync("/api/orders/analytics");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var analytics = JsonSerializer.Deserialize<OrderAnalytics>(content, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        Assert.That(analytics, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(analytics.TotalOrders, Is.GreaterThan(0));
            Assert.That(analytics.AverageOrderValue, Is.GreaterThan(0));
        });
    }

    [TearDown]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }
}