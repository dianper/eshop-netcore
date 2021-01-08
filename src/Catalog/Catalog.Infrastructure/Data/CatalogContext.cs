namespace Catalog.Infrastructure.Data
{
    using Catalog.Core.Entities;
    using Catalog.Support.Configurations;
    using MongoDB.Driver;

    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(MongoDbConfiguration mongoDbConfiguration)
        {
            var client = new MongoClient(mongoDbConfiguration.ConnectionString);
            var database = client.GetDatabase(mongoDbConfiguration.DatabaseName);

            this.Products = database.GetCollection<Product>(mongoDbConfiguration.CollectionName);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
