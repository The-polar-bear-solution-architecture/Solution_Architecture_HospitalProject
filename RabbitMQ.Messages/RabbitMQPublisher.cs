using Polly;
using RabbitMQ.Client;
using RabbitMQ.Messages;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;

namespace RabbitMQ.Infrastructure.MessagePublishers
{
    public class RabbitMQPublisher : IPublisher
    {
        // 1. Making connection to rabbitMQ
        // 2. Maintaining connection
        // 3. Sending messages.
        public ConnectionFactory ConnectionFactory { get; set; }
        public IConnection Connection { get; set; }
        public IModel Model { get; set; }

        private readonly string _the_host;
        private readonly List<string> _hosts;
        private readonly string _virtualHost;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;

        public RabbitMQPublisher(string host, string exchange, int port, string virtual_host)
        {
            Console.WriteLine($"Connecting to host: {host} and Exchange {exchange} on port {port}");
            _the_host = host;
            _hosts = new List<string>()
            {
                host
            };
            _virtualHost = virtual_host;
            _port = port;
            _exchange = exchange;
            Connect();
        }

        public Task SendMessage(string MessageType, object Message, string routingKey)
        {
            Console.WriteLine($"Message of {MessageType} has send to {routingKey}");

            Model.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout, durable: true);

            return Task.Run(() =>
            {
                byte[] data = Message.Serialize();
                var body = data;
                IBasicProperties properties = Model.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> { { "MessageType", MessageType } };
                Model.BasicPublish(_exchange, routingKey, properties, body);
            });
        }

        private void Connect()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.Error.WriteLine("Error connecting to RabbitMQ. Retrying in 5 sec."); })
                .Execute(() =>
                {
                    var factory = new ConnectionFactory() { HostName = _the_host, UserName = "guest", Password = "guest", Port=_port};
                    factory.AutomaticRecoveryEnabled = true;
                    Connection = factory.CreateConnection();
                    Model = Connection.CreateModel();

                    // TODO: Durable zal uiteindelijk naar true moeten gaan.
                    Model.ExchangeDeclare(_exchange, "fanout", durable: true, autoDelete: false);
                });
        }

        public void Dispose()
        {
            Model?.Dispose();
            Model = null;
            Connection?.Dispose();
            Connection = null;
        }

        ~RabbitMQPublisher()
        {
            Dispose();
        }
    }
}
