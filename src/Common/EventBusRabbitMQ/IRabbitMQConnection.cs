namespace EventBusRabbitMQ
{
    using RabbitMQ.Client;
    using System;

    public interface IRabbitMQConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
