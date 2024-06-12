using Polly;
using RabbitMQ.Client;
using RabbitMQ.Messages;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

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

        private const int DEFAULT_PORT = 5672;
        private const string DEFAULT_VIRTUAL_HOST = "/";

        private readonly List<string> _hosts;
        private readonly string _virtualHost;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;

        public RabbitMQPublisher(string exchange, string host,  int port)
        {
            _hosts = new List<string>()
            {
                host
            };
            _port = port;
            _exchange = exchange;
            Connect();
        }

        public RabbitMQPublisher(string exchange)
        {
            _exchange = exchange;
            _port = DEFAULT_PORT;
            _hosts = new List<string>()
            {
                "localhost"
            };
            _virtualHost = DEFAULT_VIRTUAL_HOST;
            _username = "";
            _password = "";
            Console.WriteLine($"Exchange {_exchange}, port is {DEFAULT_PORT}, virt {DEFAULT_VIRTUAL_HOST}, username is {_username}");
            Connect();
        }

        public Task SendMessage(string MessageType, object Message, string routingKey)
        {
            Console.WriteLine($"Message of {MessageType} has send to {routingKey}");

            Model.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

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
                    var factory = new ConnectionFactory() { HostName = "rabbit", UserName = "guest", Password = "guest", Port=5672};
                    factory.AutomaticRecoveryEnabled = true;
                    Connection = factory.CreateConnection();
                    Model = Connection.CreateModel();

                    // TODO: Durable zal uiteindelijk naar true moeten gaan.
                    Model.ExchangeDeclare(_exchange, "fanout", durable: false, autoDelete: false);
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
