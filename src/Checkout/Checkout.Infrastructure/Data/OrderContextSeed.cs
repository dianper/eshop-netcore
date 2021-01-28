namespace Checkout.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Checkout.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                orderContext.Database.Migrate();

                if (!orderContext.Orders.Any())
                {
                    orderContext.Orders.AddRange(GetOrders());
                    await orderContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 3)
                {
                    retryForAvailability++;

                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(ex.Message);

                    await SeedAsync(orderContext, loggerFactory, retryForAvailability);
                }

                throw;
            }
        }

        private static IEnumerable<Order> GetOrders() =>
            new List<Order>
            {
                new Order
                {
                    UserName = "username",
                    TotalPrice = 150,
                    FirstName = "FirstName",
                    LastName = "LastName",
                    Email = "email@email.com",
                    Address = "address",
                    Country = "country",
                    State = "state",
                    ZipCode = "1234-567",
                    CardName = "card name",
                    CardNumber = "1111111111111111",
                    Expiration = "10/24",
                    CVV = "123",
                    PaymentMethod = 1 // TODO: Create enum
                }
            };
    }
}
