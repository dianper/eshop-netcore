namespace Checkout.API
{
    using System.Reflection;
    using AutoMapper;
    using Checkout.API.Extensions;
    using Checkout.API.RabbitMQ;
    using Checkout.Application.Commands;
    using Checkout.Application.Handlers;
    using Checkout.Application.Mapper;
    using Checkout.Core.Repositories;
    using Checkout.Infrastructure.Data;
    using Checkout.Infrastructure.Repositories;
    using EventBusRabbitMQ;
    using global::RabbitMQ.Client;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

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
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("Order")), ServiceLifetime.Singleton);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(OrderMapper).GetTypeInfo().Assembly);

            services.AddMediatR(typeof(GetOrderByUserNameHandler).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CheckoutOrderCommand).GetTypeInfo().Assembly);

            services.AddControllers();

            services.AddSwaggerGen(sa =>
            {
                sa.SwaggerDoc("v1", new OpenApiInfo { Title = "Checkout API", Version = "v1" });
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

            services.AddSingleton<RabbitMQConsumer>();
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

            app.UseRabbitMQListener();

            app.UseSwagger();
            app.UseSwaggerUI(sa =>
            {
                sa.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout API V1");
            });
        }
    }
}
