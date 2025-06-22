namespace order_app.services
{
    public interface IDiscountService
    {
        Task<decimal> CalculateDiscountAsync(int customerId, decimal orderAmount);
    }
}
