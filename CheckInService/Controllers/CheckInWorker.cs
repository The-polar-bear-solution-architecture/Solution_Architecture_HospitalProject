using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.Mapper;
using CheckInService.Models;
using CheckInService.Models.DTO;
using CheckInService.Repositories;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using RabbitMQ.Messages.Messages;
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
            byte[] body =  message as byte[];
            switch (messageType)
            {
                case "AppointmentCreated":
                    var post_Command = body.Deserialize<CreateCheckInCommandDTO>().MapToRegister();
                    // This will create a checkin for its appointmentment
                    Event RegisterEvent = await checkInCommandHandler.RegisterCheckin(post_Command);

                    // Message type is CheckInRegistrationEvent
                    await EventStoreRepository.StoreMessage(nameof(CheckIn), RegisterEvent.MessageType, RegisterEvent);

                    // Send event to Notification service
                    Console.WriteLine("Important: Send to notification service so user will receive message evening before Appointment");
                    break;
                case "AppointmentDeleted":
                    // Will delete appointment and checkin.
                    var deleteCommand = body.Deserialize<AppointmentDeleteCommand>();
                    Event? delete_Event = await checkInCommandHandler.DeleteAppointment(deleteCommand);
                    if(delete_Event != null)
                    {
                        // Message type is AppointmentDeleteEvent
                        await EventStoreRepository.StoreMessage(nameof(CheckIn), delete_Event.MessageType, delete_Event);
                    }
                    break;
                case "AppointmentUpdated":
                    // Will update the appointment.
                    var updateCommand = body.Deserialize<UpdateCheckInDTO>();
                    Event? updateEvent = await checkInCommandHandler.UpdateAppointment(updateCommand.MapToAppointmentUpdateCommand());
                    if (updateEvent != null)
                    {
                        // Message type is AppointmentUpdateEvent
                        await EventStoreRepository.StoreMessage(nameof(CheckIn), updateEvent.MessageType, updateEvent);
                    }
                    break;
                default:
                    Console.WriteLine("No one");
                    break;
            }
            return true;
        }

        private Task handle()
        {
            Console.WriteLine("Handled by CheckIn worker");
            return Task.CompletedTask;
        }
    }
}
