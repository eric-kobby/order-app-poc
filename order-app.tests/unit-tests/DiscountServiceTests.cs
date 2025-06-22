using Moq;
using order_app.entities.Models;
using order_app.entities.Repositories;
using order_app.services.Implementations;

namespace order_app.tests.unit_tests;


public class DiscountServiceTests
{
    private Mock<ICustomerRepository> _mockCustomerRepo;
    private Mock<IOrderRepository> _mockOrderRepo;
    private DiscountService _discountService;
    
    [SetUp]
    public void Setup()
    {
        _mockCustomerRepo = new Mock<ICustomerRepository>();
        _mockOrderRepo = new Mock<IOrderRepository>();
        _discountService = new DiscountService(_mockCustomerRepo.Object, _mockOrderRepo.Object);    
    }
    
    [TestCase]
    public async Task CalculateDiscountAsync_RegularCustomerNoHistory_ReturnsZero()
    {
        // Arrange
        _mockCustomerRepo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new Customer { Id = 1, Segment = CustomerSegment.Regular });
        _mockOrderRepo.Setup(x => x.GetCustomerOrdersAsync(1))
            .ReturnsAsync([]);

        // Act
        var discount = await _discountService.CalculateDiscountAsync(1, 100m);

        // Assert
        Assert.That(discount, Is.EqualTo(0m));
    }
    
    [TestCase]
    public async Task CalculateDiscountAsync_VIPCustomerLargeOrder_ReturnsCorrectDiscount()
    {
        // Arrange
        _mockCustomerRepo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new Customer { Id = 1, Segment = CustomerSegment.VIP });
        _mockOrderRepo.Setup(x => x.GetCustomerOrdersAsync(1))
            .ReturnsAsync(Enumerable.Range(1, 15).Select(i => new Order()).ToList());

        // Act
        var discount = await _discountService.CalculateDiscountAsync(1, 1500m);

        // Assert
        // VIP (10%) + Loyalty 10+ orders (10%) + Volume $1000+ (15%) = 35%, capped at 30%
        Assert.That(discount, Is.EqualTo(450m)); // 1500 * 0.30
    }
    
    [TestCase]
    public async Task CalculateDiscountAsync_PremiumCustomerMediumOrder_ReturnsCorrectDiscount()
    {
        // Arrange
        _mockCustomerRepo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new Customer { Id = 1, Segment = CustomerSegment.Premium });
        _mockOrderRepo.Setup(x => x.GetCustomerOrdersAsync(1))
            .ReturnsAsync(Enumerable.Range(1, 7).Select(i => new Order()).ToList());

        // Act
        var discount = await _discountService.CalculateDiscountAsync(1, 300m);

        // Assert
        // Premium (5%) + Loyalty 5-9 orders (5%) + Volume $200+ (5%) = 15%
        Assert.That(discount, Is.EqualTo(45m)); // 300 * 0.15
    }
    
    [TestCase]
    public async Task CalculateDiscountAsync_NonExistentCustomer_ReturnsZero()
    {
        // Arrange
        _mockCustomerRepo.Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Customer?)null);

        // Act
        var discount = await _discountService.CalculateDiscountAsync(999, 100m);

        // Assert
        Assert.That(discount, Is.EqualTo(0m));
    }
}