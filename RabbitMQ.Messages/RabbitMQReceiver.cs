using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

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

        public List<string> _hosts {  get; set; }
        public string host { get; set; }
        public string exchange { get; set; }
        public string queue { get; set; }
        public string routingKey { get; set; }
        private string _consumerTag { get; set; }

        private string _virtual_host { get; set; }
        private int _port { get; set; }

        public RabbitMQReceiver(string host, string exchange, string queue, string routingKey, int port, string virtual_host)
        {
            _hosts = new List<string>() { host };
            this.host = host;
            this.exchange = exchange;
            this.queue = queue;
            this.routingKey = routingKey;
            _port = port;
            _virtual_host = virtual_host;
        }

        public void Start(IMessageHandleCallback messageHandleCallback)
        {
            Console.WriteLine($"Host is {host} Queue is: {queue}, routingKey is {routingKey} and Exchange is {exchange}, port is {_port}");
            _callBack = messageHandleCallback;

            Polly.Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { Console.Error.WriteLine("Error connecting to RabbitMQ. Retrying in 5 sec."); })
            .Execute(() =>
            {
                var factory = new ConnectionFactory() {
                    HostName = host,
                    Port = _port,
                    VirtualHost = _virtual_host,
                    DispatchConsumersAsync = true,
                    UserName = "guest",
                    Password = "guest"
                };

                Connection = factory.CreateConnection(_hosts);
                Model = Connection.CreateModel();

                // TODO: Durable zal uiteindelijk naar true moeten gaan.
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
            byte[] body = ea.Body.ToArray();
            // call callback to handle the message
            return _callBack.HandleMessageAsync(messageType, body);
        }
    }
}
