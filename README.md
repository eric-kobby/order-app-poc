 .NET 8 Web API implementing a comprehensive order management system with discounting, status tracking, and analytics.

## Features

### 🎯 Discounting System
- **Customer Segment Discounts**: Regular (0%), Premium (5%), VIP (10%)
- **Loyalty Discounts**: 5+ orders (5%), 10+ orders (10%)
- **Volume Discounts**: $200+ (5%), $500+ (10%), $1000+ (15%)
- **Maximum Discount**: Capped at 30%

### 📊 Order Status Tracking
- **Valid Transitions**: Pending → Processing → Shipped → Delivered
- **Cancellation**: Available for non-delivered orders
- **Automatic Fulfillment Timestamp**: Set when delivered

### 📈 Analytics Endpoint
- Average order value
- Average fulfillment time
- Total orders count
- Status distribution

## Quick Start

```bash
dotnet run
```

Navigate to `https://localhost:7xxx/swagger` for API documentation.

## API Endpoints

- `POST /api/orders` - Create order
- `PATCH /api/orders/{id}/status` - Update status
- `GET /api/orders/analytics` - Get analytics

## Performance Optimizations

1. **InMemory database** for Testing purposes without database round-trip overhead
2. **Minimal API surface** with focused DTOs
3. **Efficient LINQ queries** with proper indexing

## Testing

```bash
dotnet test
```

## Architecture

- **Clean Architecture** with separation of concerns
- **Dependency Injection** for loose coupling
- **Repository Pattern** for data access abstraction
- **Service Layer** for business logic
- **Comprehensive Swagger Documentation**
```

## Key Design Decisions

1. **In-Memory Storage**: Uses thread-safe collections for simplicity
2. **Discount Strategy**: Combines multiple discount types with a maximum cap
3. **State Machine**: Validates order status transitions
4. **Testing**: Both unit tests with mocking and integration tests
