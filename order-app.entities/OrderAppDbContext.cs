using Microsoft.EntityFrameworkCore;
using order_app.entities.Models;

namespace order_app.entities
{
    public class OrderAppDbContext(DbContextOptions<OrderAppDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public static void Seed(DbContext dbContext, bool seed)
        {
            var customers = dbContext.Set<Customer>();
            customers.AddRange(SeedCustomers());
            dbContext.SaveChanges();
        }

        public static List<Customer> SeedCustomers()
        {
            return
            [
                new Customer { Id = 1, Name = "John Doe", Segment = CustomerSegment.Regular },
                new Customer { Id = 2, Name = "Jane Smith", Segment = CustomerSegment.Premium },
                new Customer { Id = 3, Name = "Bob Wilson", Segment = CustomerSegment.VIP },
            ];
        }
    }
}
