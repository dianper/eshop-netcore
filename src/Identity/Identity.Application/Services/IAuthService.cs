namespace Identity.Application.Services
{
    using System.Threading.Tasks;
    using Identity.Application.Models;

    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(string email, string password);
    }
}
