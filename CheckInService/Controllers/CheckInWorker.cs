using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.Mapper;
using CheckInService.Models.DTO;
using CheckInService.Repositories;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using System.Text.Json;

namespace CheckInService.Controllers
{
    // This class will run on its own when using the IHostedService.
    public class CheckInWorker : IMessageHandleCallback, IHostedService
    {
        private IReceiver _messageHandler;
        private readonly CheckInCommandHandler checkInCommandHandler;

        public EventStoreRepository EventStoreRepository { get; }

        public CheckInWorker(IReceiver messageHandler, CheckInCommandHandler checkInCommandHandler, EventStoreRepository eventStoreRepository)
        {
            _messageHandler = messageHandler;
            this.checkInCommandHandler = checkInCommandHandler;
            EventStoreRepository = eventStoreRepository;
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
            Console.WriteLine(messageType);  
            Console.WriteLine("This message has been received on the Check in service");
            byte[] body =  message as byte[];

            switch (messageType)
            {
                case "AppointmentCreated":
                    var post_Command = body.Deserialize<CreateCheckInCommandDTO>().MapToRegister();
                    // This will create a checkin for its appointmentment
                    await checkInCommandHandler.RegisterCheckin(post_Command);
                    break;
                case "AppointmentDeleted":
                    // Will delete appointment and checkin.
                    var deleteCommand = body.Deserialize<AppointmentDeleteCommand>();
                    await checkInCommandHandler.DeleteAppointment(deleteCommand);
                    break;
                case "AppointmentUpdated":
                    // Will update the appointment.
                    var updateCommand = body.Deserialize<UpdateCheckInDTO>();
                    await checkInCommandHandler.UpdateAppointment(updateCommand.MapToAppointmentUpdateCommand());
                    break;
                default:
                    Console.WriteLine("No one");
                    break;
            }
            return true;
        }

        private Task handle()
        {
            Console.WriteLine("Hi");
            return Task.CompletedTask;
        }
    }
}
