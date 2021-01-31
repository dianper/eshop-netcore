namespace Checkout.API.RabbitMQ
{
    using System;
    using System.Text;
    using AutoMapper;
    using Checkout.Application.Commands;
    using EventBusRabbitMQ;
    using EventBusRabbitMQ.Constants;
    using EventBusRabbitMQ.Events;
    using global::RabbitMQ.Client;
    using global::RabbitMQ.Client.Events;
    using MediatR;
    using Newtonsoft.Json;

    public class RabbitMQConsumer
    {
        private readonly IRabbitMQConnection connection;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public RabbitMQConsumer(
            IRabbitMQConnection connection,
            IMediator mediator,
            IMapper mapper)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void Consume()
        {
            using var channel = this.connection.CreateModel();

            channel.QueueDeclare(
                queue: RabbitMQConstants.BasketCheckoutQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Consumer_Received;

            channel.BasicConsume(
                queue: RabbitMQConstants.BasketCheckoutQueue,
                autoAck: true,
                consumer: consumer);
        }

        public void Disconnect()
        {
            this.connection.Dispose();
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == RabbitMQConstants.BasketCheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);

                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                var command = this.mapper.Map<CheckoutOrderCommand>(basketCheckoutEvent);

                var orderResponse = await this.mediator.Send(command);

                /* Use orderResponse for anything */
            }
        }
    }
}
