namespace Basket.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Basket.Core.Entities;
    using Basket.Core.Repository;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly ILogger<BasketController> logger;

        public BasketController(
            IBasketRepository basketRepository,
            ILogger<BasketController> logger)
        {
            this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(basketRepository));
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetAsync(string username)
        {
            var basket = await this.basketRepository.GetAsync(username);
            if (basket == null)
            {
                this.logger.LogWarning("Basket not found");

                return NotFound();
            }

            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateAsync([FromBody] Cart basket)
        {
            return Ok(await this.basketRepository.UpdateAsync(basket));
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync(string username)
        {
            return Ok(await this.basketRepository.DeleteAsync(username));
        }
    }
}
