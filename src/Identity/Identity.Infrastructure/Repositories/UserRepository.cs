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

        public async Task<bool> IsValid(string email, string password)
        {
            return await this.authContext.Users.AnyAsync(_ => _.Email.Equals(email) && _.Password.Equals(password));
        }
    }
}
