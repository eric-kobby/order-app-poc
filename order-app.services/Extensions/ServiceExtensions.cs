using Microsoft.Extensions.DependencyInjection;
using order_app.services.Implementations;

namespace order_app.services.Extensions
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<IOrderService, OrderService>()
                    .AddScoped<IDiscountService, DiscountService>();
        }
    }
}
