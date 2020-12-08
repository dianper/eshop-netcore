namespace Identity.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Identity.API.Models;
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
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            try
            {
                var response = await this.authService.AuthenticateUser(request.Email, request.Password);
                if (response == null)
                {
                    return Unauthorized(new { status = "error", message = "user unauthorized." });
                }

                return Ok(new { status = "ok", response });
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error trying to authenticate the user.", new
                {
                    Class = nameof(AuthenticationController),
                    Method = nameof(Authenticate),
                    Err = ex.Message
                });

                return BadRequest();
            }
        }
    }
}
