namespace Checkout.Infrastructure.Data
{
    using Checkout.Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IOrderContext
    {
        DbSet<Order> Orders { get; set; }
    }
}
