namespace Identity.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Identity.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class AuthContextSeed
    {
        public static async Task SeedAsync(AuthContext authContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                authContext.Database.Migrate();
                //authContext.Database.EnsureCreated();

                if (!authContext.Users.Any())
                {
                    authContext.Users.AddRange(GetUsers());
                    await authContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if(retryForAvailability < 3)
                {
                    retryForAvailability++;
                    
                    var log = loggerFactory.CreateLogger<AuthContextSeed>();
                    log.LogError(ex.Message);

                    await SeedAsync(authContext, loggerFactory, retryForAvailability);
                }

                throw;
            }
        }

        private static IEnumerable<User> GetUsers() => new List<User>
        {
            new User() { Id = Guid.NewGuid(), Email = "user1@email.com", Name = "User One", Password = "321654o!" },
            new User() { Id = Guid.NewGuid(), Email = "user2@email.com", Name = "User Two", Password = "456123t!" },
        };
    }
}
