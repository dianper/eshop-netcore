namespace Identity.Core.Repositories
{
    using System.Threading.Tasks;
    using Identity.Core.Entities;

    public interface IUserRepository : IRepository<User>
    {
        Task<User> AuthenticateAsync(string email, string password);
    }
}
