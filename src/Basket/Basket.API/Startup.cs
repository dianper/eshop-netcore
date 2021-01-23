namespace Basket.API
{
    using AutoMapper;
    using Basket.Core.Repository;
    using Basket.Infrastructure.Data;
    using Basket.Infrastructure.Repository;
    using EventBusRabbitMQ;
    using EventBusRabbitMQ.Producer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using RabbitMQ.Client;
    using StackExchange.Redis;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(this.Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddTransient<IBasketContext, BasketContext>();
            services.AddTransient<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSwaggerGen(sa =>
            {
                sa.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });
            });

            services.AddSingleton<IRabbitMQConnection>(sp =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = this.Configuration["EventBus:HostName"],
                };

                if (!string.IsNullOrEmpty(this.Configuration["EventBus:UserName"]))
                {
                    connectionFactory.UserName = this.Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrEmpty(this.Configuration["EventBus:Password"]))
                {
                    connectionFactory.Password = this.Configuration["EventBus:Password"];
                }

                return new RabbitMQConnection(connectionFactory);
            });

            services.AddSingleton<RabbitMQProducer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(sa =>
            {
                sa.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
            });
        }
    }
}
