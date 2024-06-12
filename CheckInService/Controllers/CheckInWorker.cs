using RabbitMQ.Messages.Interfaces;

namespace CheckInService.Controllers
{
    // This class will run on its own when using the IHostedService.
    public class CheckInWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;

        public CheckInWorker(IReceiver messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Create checkin worker");
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
            Console.WriteLine("This message has been received on the Check in service");
            return true;
        }

        private Task handle()
        {
            Console.WriteLine("Hi");
            return Task.CompletedTask;
        }
    }
}
