using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.Models.DTO;
using CheckInService.Models;
using CheckInService.Repositories;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using CheckInService.Mapper;
using RabbitMQ.Infrastructure.MessageHandlers;
using CheckInService.CommandsAndEvents.Commands.CheckIn;

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
            byte[] data = message as byte[];

            // Message type is CheckInRegistrationEvent
            if (messageType.Equals("CheckInRegistrationEvent"))
            {
                var deserializedData = data.Deserialize<CheckInReadModel>();
                Console.WriteLine($"Create checkin model {deserializedData.ApointmentName}");
                // Perform operations.
                readModelRepository.Create(deserializedData);
            } else if (messageType.Equals("CheckInNoShowEvent") || messageType.Equals("CheckInPresentEvent"))
            {
                var updateCheckIn = data.Deserialize<CheckInUpdateCommand>();
                Console.WriteLine("Update read model.");
                // Perform operations.
                readModelRepository.Update(updateCheckIn);
            }
            else if (messageType.Equals("AppointmentUpdateEvent"))
            {
                var appointmentUpdate = data.Deserialize<AppointmentUpdateCommand>();
                Console.WriteLine("Update appointment part of read model.");
                // Perform operations.
                readModelRepository.Update(appointmentUpdate);
            }
            else if (messageType.Equals("AppointmentDeleteEvent"))
            {
                var appointmentDeletion = data.Deserialize<AppointmentDeleteCommand>();
                Console.WriteLine("Delete appointment");
                readModelRepository.DeleteByAppointment(appointmentDeletion.AppointmentSerialNr);
            } else if (messageType.Equals("Clear"))
            {
                readModelRepository.DeleteAll();
                Console.WriteLine("Rebuild everything appointment");
            }
            else
            {
                Console.WriteLine("Nothing?");
            }

            return true;
        }
    }

    
}
