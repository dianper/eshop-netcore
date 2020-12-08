namespace Identity.Application.Services
{
    using System.Threading.Tasks;
    using Identity.Application.Models;

    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateUser(string email, string password);
    }
}
