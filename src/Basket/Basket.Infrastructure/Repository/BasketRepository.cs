namespace Basket.Infrastructure.Repository
{
    using System;
    using System.Threading.Tasks;
    using Basket.Core.Entities;
    using Basket.Core.Repository;
    using Basket.Infrastructure.Data;
    using Newtonsoft.Json;

    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext basketContext;

        public BasketRepository(IBasketContext basketContext)
        {
            this.basketContext = basketContext ?? throw new ArgumentNullException(nameof(basketContext));
        }

        public async Task<bool> DeleteAsync(string userName)
        {
            return await this.basketContext.Redis.KeyDeleteAsync(userName);
        }

        public async Task<Cart> GetAsync(string userName)
        {
            var basket = await this.basketContext.Redis.StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<Cart>(basket);
        }

        public async Task<Cart> UpdateAsync(Cart basket)
        {
            var updated = await this.basketContext.Redis.StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            if (!updated)
            {
                return null;
            }

            return await GetAsync(basket.UserName);
        }
    }
}
