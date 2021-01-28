namespace EventBusRabbitMQ.Producer
{
    using System;
    using System.Text;
    using EventBusRabbitMQ.Events;
    using Newtonsoft.Json;
    using RabbitMQ.Client.Events;

    public class RabbitMQProducer
    {
        private readonly IRabbitMQConnection connection;

        public RabbitMQProducer(IRabbitMQConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent basketCheckoutEvent)
        {
            using var channel = this.connection.CreateModel();
            channel.QueueDeclare(queueName, false, false, false, null);

            var message = JsonConvert.SerializeObject(basketCheckoutEvent);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;

            channel.ConfirmSelect();
            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                mandatory: true,
                basicProperties: null,
                body: body);

            channel.WaitForConfirmsOrDie();
            channel.BasicAcks += Channel_BasicAcks;
            channel.ConfirmSelect();
        }

        private void Channel_BasicAcks(object sender, BasicAckEventArgs e)
        {
            Console.WriteLine("Sending RabbitMQ...");
        }
    }
}
