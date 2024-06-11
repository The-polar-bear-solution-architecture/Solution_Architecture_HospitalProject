using RabbitMQ.Messages.Interfaces;

namespace CheckInService.CheckInWorker
{
    public class CheckInWorker : IMessageHandleCallback, IHostedService
    {
        public Task<bool> HandleMessageAsync(string messageType, object message)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
