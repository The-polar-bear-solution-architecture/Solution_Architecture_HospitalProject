using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
        public IMessageHandleCallback _callBack { get; set; }
        private AsyncEventingBasicConsumer Consumer;

        public string host { get; set; }
        public string exchange { get; set; }
        public string queue { get; set; }
        public string routingKey { get; set; }
        private string _consumerTag { get; set; }

        public RabbitMQReceiver(string host, string exchange, string queue, string routingKey) {
            this.host = host;
            this.exchange = exchange;
            this.queue = queue;
            this.routingKey = routingKey;
        }

        public void Start(IMessageHandleCallback messageHandleCallback)
        {
            _callBack = messageHandleCallback;
            Polly.Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.Error.WriteLine("Error connecting to RabbitMQ. Retrying in 5 sec."); })
            .Execute(() =>
            {
                var factory = new ConnectionFactory() {  DispatchConsumersAsync = true, HostName = "localhost" };
                Connection = ConnectionFactory.CreateConnection();
                Model = Connection.CreateModel();
                Model.ExchangeDeclare(exchange, "fanout", durable: true, autoDelete: false);
                Model.QueueDeclare(queue, durable: true, autoDelete: false, exclusive: false);
                Model.QueueBind(queue, exchange, routingKey);
                Consumer = new AsyncEventingBasicConsumer(Model);
                Consumer.Received += Consumer_Received;
                _consumerTag = Model.BasicConsume(queue, false, Consumer);
            });
        }

        public void Stop()
        {
            Model.BasicCancel(_consumerTag);
            Model.Close(200, "Goodbye");
            Connection.Close();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            if (await HandleEvent(ea))
            {
                Model.BasicAck(ea.DeliveryTag, false);
            }
        }

        private Task<bool> HandleEvent(BasicDeliverEventArgs ea)
        {
            // determine messagetype
            string messageType = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["MessageType"]);

            // get body
            string body = Encoding.UTF8.GetString(ea.Body.ToArray());

            // call callback to handle the message
            return _callBack.HandleMessageAsync(messageType, body);
        }
    }
}
