namespace Catalog.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Catalog.Core.Entities;
    using Catalog.Core.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    //[Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ILogger<CatalogController> logger;

        public CatalogController(
            IProductRepository productRepository,
            ILogger<CatalogController> logger)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllAsync()
        {
            var products = await this.productRepository.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetByIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetByIdAsync(string id)
        {
            var product = await this.productRepository.GetByIdAsync(id);

            if (product == null)
            {
                this.logger.LogWarning($"Product id not found.", new { ProductId = id });

                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategoryAsync(string category)
        {
            var products = await this.productRepository.GetAllByCategoryAsync(category);

            return Ok(products);
        }

        [Route("[action]/{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetByNameAsync(string name)
        {
            var products = await this.productRepository.GetAllByNameAsync(name);

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateAsync([FromBody] Product product)
        {
            await this.productRepository.CreateAsync(product);

            return CreatedAtRoute("GetByIdAsync", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync([FromBody] Product product)
        {
            return Ok(await this.productRepository.UpdateAsync(product));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            return Ok(await this.productRepository.DeleteAsync(id));
        }
    }
}
