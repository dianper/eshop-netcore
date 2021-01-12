namespace Catalog.Core.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catalog.Core.Entities;

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        
        Task<IEnumerable<Product>> GetAllByNameAsync(string name);
        
        Task<IEnumerable<Product>> GetAllByCategoryAsync(string category);
        
        Task<Product> GetByIdAsync(string id);
        
        Task CreateAsync(Product product);
        
        Task<bool> UpdateAsync(Product product);
        
        Task<bool> DeleteAsync(string id);
    }
}
