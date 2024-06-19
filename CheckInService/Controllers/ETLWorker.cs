using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.Models.DTO;
using CheckInService.Models;
using CheckInService.Repositories;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using CheckInService.Mapper;
using RabbitMQ.Infrastructure.MessageHandlers;

namespace CheckInService.Controllers
{
    public class ETLWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;
        private readonly ReadModelRepository readModelRepository;

        public ETLWorker(ReadModelRepository readModelRepository, IConfiguration configuration)
        {
            var section = configuration.GetSection("RabbitMQHandler");
            string customRoutingKey = "ETL_Checkin";
            int port = section.GetValue<int>("Port");
            string _host = section.GetValue<string>("Host");
            string _exchange = section.GetValue<string>("Exchange");
            string queue = section.GetValue<string>("Queue");
            IReceiver receiver = new RabbitMQReceiver(_host, _exchange, "CustomQueue", customRoutingKey, port, "/");
            _messageHandler = receiver;
            this.readModelRepository = readModelRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Create ETL Worker");
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
            // Message type is CheckInRegistrationEvent
            // Message type is CheckInNoShowEvent
            // Message type is CheckInPresentEvent
            // Message type is AppointmentUpdateEvent
            // Message type is AppointmentDeleteEvent
            Console.WriteLine("ETL Worker works");
            return true;
        }
    }

    
}
