namespace Identity.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Identity.Core.Entities;
    using Identity.Core.Repositories;
    using Identity.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AuthContext authContext) : base(authContext)
        {
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            return await this.authContext.Users.FirstOrDefaultAsync(_ => _.Email.Equals(email) && _.Password.Equals(password));
        }
    }
}
