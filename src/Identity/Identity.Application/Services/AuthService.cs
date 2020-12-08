namespace Identity.Application.Services
{
    using System;
    using System.Threading.Tasks;
    using Identity.Application.Models;
    using Identity.Core.Repositories;

    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;

        public AuthService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<AuthResponse> AuthenticateUser(string email, string password)
        {
            if (await this.userRepository.IsValid(email, password))
            {
                // Todo: Generate token via JWT
                return new AuthResponse { Token = "ok", Expiration = DateTime.UtcNow.AddMinutes(60) };
            }

            return null;
        }
    }
}
