using Polly;
using RabbitMQ.Client;
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

        public string Exchange { get; set; }

        private const int DEFAULT_PORT = 5672;
        private const string DEFAULT_VIRTUAL_HOST = "/";

        private readonly List<string> _hosts;
        private readonly string _virtualHost;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _exchange;

        

        public void SendMessage(string MessageType, object Message, string routingKey)
        {
            Model.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            PublishMessageAsync("Hello", Message, routingKey);
            /* Model.BasicPublish(exchange: Exchange,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: MessageBody); */

            Console.WriteLine($"Message of {MessageType} has send to {routingKey}");
        }

        public RabbitMQPublisher(string exchange, int port)
        {
            _hosts = new List<string>()
            {
                "localhost"
            };
            _port = port;
            _exchange = exchange;
            Connect();
        }

        public RabbitMQPublisher()
        {
            Exchange = "logs";
            ConnectionFactory = new ConnectionFactory { HostName = "localhost" };
            Connection = ConnectionFactory.CreateConnection();
            Model = Connection.CreateModel();
        }

        public Task PublishMessageAsync(string messageType, object message, string routingKey)
        {
            return Task.Run(() =>
            {
                byte[] data = message.Serialize();
                var body = data;
                IBasicProperties properties = Model.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> { { "MessageType", messageType } };
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
                    var factory = new ConnectionFactory() { VirtualHost = _virtualHost, UserName = _username, Password = _password, Port = _port };
                    factory.AutomaticRecoveryEnabled = true;
                    Connection = factory.CreateConnection(_hosts);
                    Model = Connection.CreateModel();
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
