namespace Identity.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using System.Threading.Tasks;
    using Identity.Application.Models;
    using Identity.Core.Repositories;
    using Microsoft.IdentityModel.Tokens;

    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly AuthConfiguration authConfiguration;

        public AuthService(
            IUserRepository userRepository,
            AuthConfiguration authConfiguration)
        {
            this.userRepository = userRepository;
            this.authConfiguration = authConfiguration;
        }

        public async Task<AuthResponse> AuthenticateAsync(string email, string password)
        {
            var user = await this.userRepository.AuthenticateAsync(email, password);
            if (user == null)
            {
                return new AuthResponse(new Dictionary<string, string> { { "auth", "user unauthorized." } });
            }

            return new AuthResponse
            {
                Expiration = DateTime.UtcNow.AddMinutes(this.authConfiguration.ExpirationInMinutes),
                Token = this.GenerateToken(),
                UserId = user.Id,
                UserEmail = user.Email
            };
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.authConfiguration.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: this.authConfiguration.Issuer,
                audience: this.authConfiguration.Audience,
                claims: null,
                expires: DateTime.Now.AddMinutes(this.authConfiguration.ExpirationInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
