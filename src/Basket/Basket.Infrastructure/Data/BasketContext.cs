namespace Basket.Infrastructure.Data
{
    using StackExchange.Redis;

    public class BasketContext : IBasketContext
    {
        public BasketContext(ConnectionMultiplexer redisConnection)
        {
            this.Redis = redisConnection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}
