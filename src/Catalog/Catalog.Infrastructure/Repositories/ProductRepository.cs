namespace Catalog.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catalog.Core.Entities;
    using Catalog.Core.Repositories;
    using Catalog.Infrastructure.Data;
    using MongoDB.Driver;

    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public async Task CreateAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this.catalogContext.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetAsync(string id)
        {
            return await this.catalogContext.Products.Find(_ => _.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> GetByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
