using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.Configurations;
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
        private readonly IPublisher publisher;
        private readonly IPublisher InternalPublisher;
        private readonly CheckInCommandHandler checkInCommandHandler;

        public EventStoreRepository EventStoreRepository { get; }
        private readonly string RouterKey;

        public CheckInWorker(
            IReceiver messageHandler,
            IPublisher publisher,
            IRabbitFactory rabbitFactory,
            CheckInCommandHandler checkInCommandHandler, 
            EventStoreRepository eventStoreRepository)
        {
            _messageHandler = messageHandler;
            this.publisher = publisher;
            this.checkInCommandHandler = checkInCommandHandler;
            EventStoreRepository = eventStoreRepository;

            InternalPublisher = rabbitFactory.CreateInternalPublisher();
            RouterKey = "ETL_Checkin";
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
            byte[] body = message as byte[];
            switch (messageType)
            {
                case "AppointmentCreated":
                    var post_Command = body.Deserialize<CreateCheckInCommandDTO>().MapToRegister();
                    // This will create a checkin for its appointmentment
                    CheckInRegistrationEvent? RegisterEvent = await checkInCommandHandler.RegisterCheckin(post_Command);
                    if(RegisterEvent != null)
                    {
                        // Message type is CheckInRegistrationEvent
                        await EventStoreRepository.StoreMessage(nameof(CheckIn), RegisterEvent.MessageType, RegisterEvent);

                        // Send new appointment to read model.
                        await InternalPublisher.SendMessage(RegisterEvent.MessageType, RegisterEvent, RouterKey);

                        // Send event to Notification service
                        Console.WriteLine("Important: Send to notification service so user will receive message evening before Appointment");
                    }
                    break;
                case "AppointmentDeleted":
                    // Will delete appointment and checkin.
                    var deleteCommand = body.Deserialize<AppointmentDeleteCommand>();
                    AppointmentDeleteEvent? delete_Event = await checkInCommandHandler.DeleteAppointment(deleteCommand);
                    if(delete_Event != null)
                    {
                        // Message type is AppointmentDeleteEvent
                        await EventStoreRepository.StoreMessage(nameof(CheckIn), delete_Event.MessageType, delete_Event);

                        // Send delete request to ETL.
                        await InternalPublisher.SendMessage(delete_Event.MessageType, delete_Event, RouterKey);
                    }
                    break;
                case "AppointmentUpdated":
                    // Will update the appointment.
                    var updateCommand = body.Deserialize<UpdateCheckInDTO>();
                    AppointmentUpdateEvent? updateEvent = await checkInCommandHandler.UpdateAppointment(updateCommand.MapToAppointmentUpdateCommand());
                    if (updateEvent != null)
                    {
                        // Message type is AppointmentUpdateEvent
                        await EventStoreRepository.StoreMessage(nameof(CheckIn), updateEvent.MessageType, updateEvent);

                        // Send delete request to ETL.
                        await InternalPublisher.SendMessage(updateEvent.MessageType, updateEvent, RouterKey);
                    }
                    break;
                default:
                    Console.WriteLine("No one");
                    break;
            }
            return true;
        }
    }
}
