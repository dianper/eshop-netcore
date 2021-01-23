namespace Checkout.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Checkout.Core.Entities;

    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUsername(string username);
    }
}
