namespace Checkout.API.Extensions
{
    using Checkout.API.RabbitMQ;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ApplicationBuilderExtensions
    {
        public static RabbitMQConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitMQListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RabbitMQConsumer>();

            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        public static void OnStarted() => Listener.Consume();

        public static void OnStopping() => Listener.Disconnect();
    }
}
