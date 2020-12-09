namespace Identity.API.Configurations
{
    using Identity.Application.Models;

    public class AppConfiguration
    {
        public AuthConfiguration Authentication { get; set; }
        public SqlServerConfiguration SqlServer { get; set; }
    }
}
