namespace Checkout.Infrastructure.Data
{
    using Checkout.Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public class OrderContext : DbContext, IOrderContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
