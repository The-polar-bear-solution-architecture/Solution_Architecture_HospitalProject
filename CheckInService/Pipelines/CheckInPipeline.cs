using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.DBContexts;
using CheckInService.Models;
using CheckInService.Repositories;
using EventStore.Client;
using RabbitMQ.Messages.Mapper;
using RabbitMQ.Messages.Messages;

namespace CheckInService.Pipelines
{
    public class CheckInPipeline
    {
        private readonly EventStoreClient client;
        private readonly CheckInContextDB checkInContext;
        private readonly ReplayHandler replayEventHandler;
        private readonly ReadModelRepository readModelRepository;
        private readonly CheckInCommandHandler checkInCommandHandler;

        public CheckInPipeline(
            EventStoreClient client, 
            CheckInContextDB checkInContext, 
            ReplayHandler replayEventHandler, 
            ReadModelRepository readModelRepository, 
            CheckInCommandHandler checkInCommandHandler)
        {
            this.client = client;
            this.checkInContext = checkInContext;
            this.replayEventHandler = replayEventHandler;
            this.readModelRepository = readModelRepository;
            this.checkInCommandHandler = checkInCommandHandler;
        }

        public async Task RunPipeline()
        {
            Console.WriteLine("==== Run pipeline ====");
            Console.WriteLine("==== Get from write database pipeline ====");
            List<Message> list = new List<Message>();
            var result = client.ReadStreamAsync(
                Direction.Forwards,
                nameof(CheckIn),
                StreamPosition.Start,
                resolveLinkTos: true
            );
            var events = await result.ToListAsync();
            foreach (var command in events)
            {
                string EventType = command.OriginalEvent.EventType;
                byte[] data = command.OriginalEvent.Data.ToArray();
                Message? entity_event = null;
                Console.WriteLine(EventType);
                // Hier zullen wijzigingen doorgevoerd moeten worden.
                switch (EventType)
                {
                    case nameof(CheckInNoShowEvent):
                        entity_event = data.Deserialize<NoShowCheckIn>();
                        var updateCheckIn = data.Deserialize<CheckInUpdateCommand>();
                        await checkInCommandHandler.ChangeToNoShow((NoShowCheckIn)entity_event);
                        // Readdb act
                        readModelRepository.Update(updateCheckIn);
                        break;
                    case nameof(CheckInPresentEvent):
                        entity_event = data.Deserialize<PresentCheckin>();
                        var presentCheckIn = data.Deserialize<CheckInUpdateCommand>();
                        await checkInCommandHandler.ChangeToPresent((PresentCheckin)entity_event);
                        // Readdb act
                        readModelRepository.Update(presentCheckIn);
                        break;
                    case nameof(CheckInRegistrationEvent):
                        RegisterCheckin registerCommand = data.Deserialize<RegisterCheckin>();
                        CheckInReadModel readModel = data.Deserialize<CheckInReadModel>();
                        await checkInCommandHandler.RegisterCheckin(registerCommand);
                        // Readdb act
                        readModelRepository.Create(readModel);
                        break;
                    case nameof(AppointmentDeleteEvent):
                        var delete_command = data.Deserialize<AppointmentDeleteCommand>();
                        await checkInCommandHandler.DeleteAppointment(delete_command);
                        // Readdb act
                        readModelRepository.DeleteByAppointment(delete_command.AppointmentSerialNr);
                        break;
                    case nameof(AppointmentUpdateEvent):
                        var update_command = data.Deserialize<AppointmentUpdateCommand>();
                        await checkInCommandHandler.UpdateAppointment(update_command);
                        // Readdb act
                        var appointmentUpdate = data.Deserialize<AppointmentReadUpdateCommand>();
                        // Perform operations.
                        readModelRepository.Update(appointmentUpdate);
                        break;
                    default:
                        Console.WriteLine($"No object convertion possible with {EventType}");
                        break;
                }
            }
            Console.WriteLine("==== Data is synchronised ====");
        }
    
        public async Task SynchroniseWriteDBWithReadDB()
        {
            Console.WriteLine("Start synchronisation");
        }
    }
}
