namespace Identity.Core.Repositories
{
    using System.Threading.Tasks;
    using Identity.Core.Entities;

    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsValid(string email, string password);
    }
}
