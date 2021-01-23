namespace Basket.API.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using AutoMapper;
    using Basket.Core.Entities;
    using Basket.Core.Repository;
    using EventBusRabbitMQ.Constants;
    using EventBusRabbitMQ.Events;
    using EventBusRabbitMQ.Producer;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;
        private readonly ILogger<BasketController> logger;
        private readonly RabbitMQProducer eventBus;

        public BasketController(
            IBasketRepository basketRepository,
            IMapper mapper,
            ILogger<BasketController> logger,
            RabbitMQProducer eventBus)
        {
            this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(basketRepository));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetAsync(string username)
        {
            var basket = await this.basketRepository.GetAsync(username);
            return Ok(basket ?? new Cart(username));
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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckoutAsync([FromBody] Checkout checkout)
        {
            var basket = await this.basketRepository.GetAsync(checkout.Username);
            if (basket == null)
            {
                return BadRequest();
            }

            var basketRemoved = await this.basketRepository.DeleteAsync(basket.Username);
            if (!basketRemoved)
            {
                return BadRequest();
            }

            var eventMessage = this.mapper.Map<BasketCheckoutEvent>(checkout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                this.eventBus.PublishBasketCheckout(RabbitMQConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error publishing integration event.", new
                {
                    Class = nameof(BasketController),
                    Method = nameof(CheckoutAsync),
                    RequestId = eventMessage.RequestId,
                    Ex = ex
                });

                throw;
            }

            return Accepted();
        }
    }
}
