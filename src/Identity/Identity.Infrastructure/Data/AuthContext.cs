namespace Identity.Infrastructure.Data
{
    using Identity.Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public class AuthContext : DbContext, IAuthContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
