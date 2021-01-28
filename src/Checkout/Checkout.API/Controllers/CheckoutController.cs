namespace Checkout.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Checkout.Application.Commands;
    using Checkout.Application.Queries;
    using Checkout.Application.Responses;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator mediator;

        public CheckoutController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserNameAsync(string userName)
        {
            var query = new GetOrderByUserNameQuery(userName);

            return Ok(await this.mediator.Send(query));
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderResponse>> CheckoutOrderAsync([FromBody] CheckoutOrderCommand command)
        {
            var result = await this.mediator.Send(command);

            return Ok(result);
        }
    }
}
