using RabbitMQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHandler
{
    public class HostingWorker : IHostedService, IMessageHandleCallback
    {
        private const decimal HOURLY_RATE = 18.50M;
        private IReceiver _messageHandler;

        public HostingWorker(IReceiver messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, object message)
        {
            await handle();
            Console.WriteLine("Message object received");
            return true;
        }

        private Task handle()
        {
            Console.WriteLine("Hi");
            return Task.CompletedTask;
        }
    }
}
