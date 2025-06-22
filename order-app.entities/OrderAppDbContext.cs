using Microsoft.EntityFrameworkCore;
using order_app.entities.Models;

namespace order_app.entities
{
    public class OrderAppDbContext : DbContext
    {
        public OrderAppDbContext(DbContextOptions<OrderAppDbContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
