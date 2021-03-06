﻿namespace Basket.API.Controllers
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
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetAsync(string userName)
        {
            var basket = await this.basketRepository.GetAsync(userName);
            return Ok(basket ?? new Cart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateAsync([FromBody] Cart basket)
        {
            return Ok(await this.basketRepository.UpdateAsync(basket));
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync(string userName)
        {
            return Ok(await this.basketRepository.DeleteAsync(userName));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckoutAsync([FromBody] Checkout checkout)
        {
            var basket = await this.basketRepository.GetAsync(checkout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            var basketRemoved = await this.basketRepository.DeleteAsync(basket.UserName);
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
