namespace Checkout.Infrastructure.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Checkout.Core.Entities;
    using Checkout.Core.Repositories;
    using Checkout.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext) : base(orderContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            return await this.orderContext.Orders.Where(_ => _.UserName.Equals(userName)).ToListAsync();
        }
    }
}
