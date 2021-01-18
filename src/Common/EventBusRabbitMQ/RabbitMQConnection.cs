namespace EventBusRabbitMQ
{
    using System;
    using System.Threading;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Exceptions;

    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory connectionFactory;
        private IConnection connection;
        private bool disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

            if (!this.IsConnected)
            {
                this.TryConnect();
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.connection != null && this.connection.IsOpen && !disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!this.IsConnected)
            {
                throw new InvalidOperationException("No rabbit connection");
            }

            return this.connection.CreateModel();
        }

        public bool TryConnect()
        {
            try
            {
                this.connection = this.connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);

                this.connection = this.connectionFactory.CreateConnection();
            }

            if (this.IsConnected)
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            try
            {
                this.connection.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
