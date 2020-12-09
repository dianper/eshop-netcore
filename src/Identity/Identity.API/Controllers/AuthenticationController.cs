namespace Identity.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Identity.Application.Models;
    using Identity.Application.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger<AuthenticationController> logger;

        public AuthenticationController(
            IAuthService authService,
            ILogger<AuthenticationController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthRequest request)
        {
            try
            {
                var response = await this.authService.AuthenticateAsync(request.Email, request.Password);
                if (!response.Success)
                {
                    return Unauthorized(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error trying to authenticate the user.", new
                {
                    Class = nameof(AuthenticationController),
                    Method = nameof(AuthenticateAsync),
                    Err = ex.Message
                });

                return BadRequest();
            }
        }
    }
}
