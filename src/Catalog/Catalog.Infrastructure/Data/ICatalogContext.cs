namespace Catalog.Infrastructure.Data
{
    using Catalog.Core.Entities;
    using MongoDB.Driver;

    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
