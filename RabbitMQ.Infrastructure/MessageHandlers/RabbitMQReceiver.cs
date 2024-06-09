using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQ.Infrastructure.MessageHandlers
{
    public class RabbitMQReceiver : IReceiver
    {
        // 1. Making connection to rabbitMQ
        // 2. Maintaining connection
        // 3. Receiving messages from a queue.

        public ConnectionFactory ConnectionFactory { get; set; }
        public IConnection Connection { get; set; }
        public IModel Model { get; set; }

        public RabbitMQReceiver() {
            ConnectionFactory = new ConnectionFactory { HostName = "localhost" };
            Connection = ConnectionFactory.CreateConnection();
            Model = Connection.CreateModel();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void StartReceiving()
        {
            throw new NotImplementedException();
        }
    }
}
