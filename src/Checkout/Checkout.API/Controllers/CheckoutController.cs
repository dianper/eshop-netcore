namespace Checkout.API.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(ILogger<CheckoutController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { };
        }
    }
}
