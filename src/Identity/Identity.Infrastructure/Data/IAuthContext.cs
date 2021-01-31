namespace Identity.Infrastructure.Data
{
    using Identity.Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IAuthContext
    {
        DbSet<User> Users { get; set; }
    }
}
