using RabbitMQ.Client;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using System;
using System.Collections.Generic;
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

        public RabbitMQPublisher() {
            Exchange = "logs";
            ConnectionFactory = new ConnectionFactory { HostName = "localhost" };
            Connection = ConnectionFactory.CreateConnection();
            Model = Connection.CreateModel();
        }

        public void SendMessage(string MessageType, object Message, string routingKey)
        {
            Model.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            var MessageBody = Message.Serialize();

            Model.BasicPublish(exchange: Exchange,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: MessageBody);

            Console.WriteLine($"Message of {MessageType} has send to {routingKey}");
        }
    }
}
