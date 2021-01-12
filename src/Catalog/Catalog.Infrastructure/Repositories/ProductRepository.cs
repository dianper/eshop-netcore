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
            await this.catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(_ => _.Id, id);

            var deleteResult = await this.catalogContext
                .Products
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this.catalogContext
                .Products
                .Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(_ => _.Id, id);

            return await this.catalogContext
                .Products
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByCategoryAsync(string category)
        {
            var filter = Builders<Product>.Filter.Eq(_ => _.Category, category);

            return await this.catalogContext
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter.ElemMatch(_ => _.Name, name);

            return await this.catalogContext
                .Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            var updateResult = await this.catalogContext
                .Products
                .ReplaceOneAsync(filter: _ => _.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
