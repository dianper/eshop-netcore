namespace Catalog.Infrastructure.Data
{
    using System.Collections.Generic;
    using Catalog.Core.Entities;
    using MongoDB.Driver;

    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(_ => true).Any();
            if (!existProduct)
            {
                productCollection.InsertMany(GetProducts());
            }
        }

        private static IEnumerable<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "Product 1",
                    Category = "Category 1",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image File",
                    Price = 10
                },
                new Product
                {
                    Name = "Product 2",
                    Category = "Category 2",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image File",
                    Price = 20
                },
                new Product
                {
                    Name = "Product 3",
                    Category = "Category 3",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "Image File",
                    Price = 30
                }
            };
        }
    }
}
