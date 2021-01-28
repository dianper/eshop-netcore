namespace Basket.Core.Repository
{
    using System.Threading.Tasks;
    using Basket.Core.Entities;

    public interface IBasketRepository
    {
        Task<Cart> GetAsync(string userName);
        Task<Cart> UpdateAsync(Cart basket);
        Task<bool> DeleteAsync(string userName);
    }
}
