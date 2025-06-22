using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using order_app.entities.Repositories;
using order_app.entities.Repositories.Implementations;

namespace order_app.entities.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            return services.AddDbContext<OrderAppDbContext>(option => option.UseInMemoryDatabase("OrderAppDb"))
                           .AddScoped<IOrderRepository, OrderRepository>()
                           .AddScoped<ICustomerRepository, CustomerRepository>();
        }
    }
}
