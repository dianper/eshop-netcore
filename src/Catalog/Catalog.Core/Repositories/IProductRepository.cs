namespace Catalog.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catalog.Core.Entities;

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(string id);
        Task<Product> GetByNameAsync(string name);
        Task<Product> GetByCategoryAsync(string category);
        Task CreateAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
    }
}
