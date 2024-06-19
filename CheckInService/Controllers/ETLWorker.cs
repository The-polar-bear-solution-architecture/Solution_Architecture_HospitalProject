using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.Models.DTO;
using CheckInService.Models;
using CheckInService.Repositories;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using CheckInService.Mapper;

namespace CheckInService.Controllers
{
    public class ETLWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;

        public ETLWorker(IReceiver messageHandler)
        {
            _messageHandler = messageHandler;
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
            byte[] body = message as byte[];
            switch (messageType)
            {
                case "AppointmentCreated":
                    var post_Command = body.Deserialize<CreateCheckInCommandDTO>().MapToRegister();
                    // This will create a checkin for its appointmentment
                    Console.WriteLine("Important: Send to notification service so user will receive message evening before Appointment");
                    break;
                case "AppointmentDeleted":
                    // Will delete appointment and checkin.
                    Console.WriteLine("Important: Send to notification service so user will receive message evening before Appointment");
                    break;
                case "AppointmentUpdated":
                    // Will update the appointment.
                    Console.WriteLine("Important: Send to notification service so user will receive message evening before Appointment");
                    break;
                default:
                    Console.WriteLine("Not found in");
                    break;
            }
            return true;
        }
    }

    
}
