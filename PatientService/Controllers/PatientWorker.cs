using RabbitMQ.Infrastructure.MessageHandlers;
using RabbitMQ.Messages.Interfaces;

namespace PatientService.Controllers
{
    public class PatientWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;
        private IPublisher publisher;

        public PatientWorker(IReceiver messageHandler, IPublisher publisher)
        {
            _messageHandler = messageHandler;
            this.publisher = publisher;
        }


        public async Task<bool> HandleMessageAsync(string messageType, object message)
        {
            Console.WriteLine("Message received");
            return true;
        }

        public  async Task SendMessageAsync(string messageType, object message)
        {
            if (messageType == "POST")
            {
                await publisher.SendMessage(messageType, message, "Created_Patient");
            }
            if(messageType == "PUT")
            {
                await publisher.SendMessage(messageType, message, "Updated_Patient");
            }
            if(messageType == "DELETE")
            {
                await publisher.SendMessage(messageType, message, "Deleted_Patient");
            }
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
    }
}
