using RabbitMQ.Infrastructure.MessageHandlers;
using RabbitMQ.Messages.Interfaces;

namespace PatientService.Controllers
{
    public class PatientWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;

        public PatientWorker(IReceiver messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public Task<bool> HandleMessageAsync(string messageType, object message)
        {
            Console.WriteLine("Message received");
            return Task.FromResult(true);
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
