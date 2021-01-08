namespace Catalog.API.Configurations
{
    using Catalog.Support.Configurations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CatalogConfiguration
    {
        public AuthConfiguration Authentication { get; set; }
        public MongoDbConfiguration MongoDb { get; set; }
    }
}
