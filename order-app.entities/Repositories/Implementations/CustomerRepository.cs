using order_app.entities.Models;

namespace order_app.entities.Repositories.Implementations
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(OrderAppDbContext ctx) : base(ctx)
        {
        }
    }
}
