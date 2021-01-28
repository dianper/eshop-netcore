namespace Identity.API
{
    using FluentValidation.AspNetCore;
    using Identity.API.Configurations;
    using Identity.API.Validators;
    using Identity.Application.Services;
    using Identity.Core.Repositories;
    using Identity.Infrastructure.Data;
    using Identity.Infrastructure.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    public class Startup
    {
        private readonly IdentityConfiguration identityConfiguration;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.identityConfiguration = configuration.Get<IdentityConfiguration>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.identityConfiguration.Authentication);

            services.AddDbContext<AuthContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("Identity")), ServiceLifetime.Singleton);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<AuthValidator>();
            });

            services.AddSwaggerGen(sa =>
            {
                sa.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(sa =>
            {
                sa.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API V1");
            });
        }
    }
}
