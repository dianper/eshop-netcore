namespace Identity.API.Configurations
{
    using Identity.Application.Models;

    public class IdentityConfiguration
    {
        public AuthConfiguration Authentication { get; set; }
        public SqlServerConfiguration SqlServer { get; set; }
    }
}
