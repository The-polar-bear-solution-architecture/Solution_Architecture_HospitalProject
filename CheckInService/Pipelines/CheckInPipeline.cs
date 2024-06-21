using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.DBContexts;
using CheckInService.Mapper;
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
        private readonly EventStoreRepository eventStoreRepository;
        private readonly CheckInContextDB checkInContext;
        private readonly ReadModelRepository readModelRepository;
        private readonly CheckInCommandHandler checkInCommandHandler;
        private readonly CheckInRepository CheckInRepository;

        public CheckInPipeline(
            EventStoreClient client, 
            EventStoreRepository eventStoreRepository,
            CheckInContextDB checkInContext, 
            ReadModelRepository readModelRepository, 
            CheckInCommandHandler checkInCommandHandler,
            CheckInRepository checkInRepository)
        {
            this.client = client;
            this.eventStoreRepository = eventStoreRepository;
            this.checkInContext = checkInContext;
            this.readModelRepository = readModelRepository;
            this.checkInCommandHandler = checkInCommandHandler;
            this.CheckInRepository = checkInRepository;
        }

        // Transports data from Event source to WriteDB and ReadDB.
        public async Task ReplayDataPipeline()
        {
            Console.WriteLine("==== Run pipeline ====");
            // Extract all data from event source.
            List<Message> list = new List<Message>();
            var events = await eventStoreRepository.GetFromCollection(nameof(CheckIn));
            // Delete all data from write database.
            await checkInCommandHandler.ClearAll();

            foreach (var command in events)
            {
                string EventType = command.OriginalEvent.EventType;
                byte[] data = command.OriginalEvent.Data.ToArray();
                Message? entity_event = null;
                // Hier zullen wijzigingen doorgevoerd moeten worden.
                switch (EventType)
                {
                    case nameof(CheckInNoShowEvent):
                        entity_event = data.Deserialize<NoShowCheckIn>();
                        var updateCheckIn = data.Deserialize<CheckInUpdateCommand>();
                        // Write to write db.
                        await checkInCommandHandler.ChangeToNoShow((NoShowCheckIn)entity_event);
                        // Readdb act
                        readModelRepository.Update(updateCheckIn);
                        break;
                    case nameof(CheckInPresentEvent):
                        entity_event = data.Deserialize<PresentCheckin>();
                        var presentCheckIn = data.Deserialize<CheckInUpdateCommand>();
                        // Write to write db.
                        await checkInCommandHandler.ChangeToPresent((PresentCheckin)entity_event);
                        // Readdb act
                        readModelRepository.Update(presentCheckIn);
                        break;
                    case nameof(CheckInRegistrationEvent):
                        RegisterCheckin registerCommand = data.Deserialize<RegisterCheckin>();
                        CheckInReadModel readModel = data.Deserialize<CheckInReadModel>();
                        // Write to write db.
                        await checkInCommandHandler.RegisterCheckin(registerCommand);
                        // Readdb act
                        readModelRepository.Create(readModel);
                        break;
                    case nameof(AppointmentDeleteEvent):
                        var delete_command = data.Deserialize<AppointmentDeleteCommand>();
                        // Write to write db.
                        await checkInCommandHandler.DeleteAppointment(delete_command);
                        // Readdb act
                        readModelRepository.DeleteByAppointment(delete_command.AppointmentId);
                        break;
                    case nameof(AppointmentUpdateEvent):
                        var update_command = data.Deserialize<AppointmentUpdateCommand>();
                        // Write to write db.
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

        // Transports data from WriteDB to ReadDB.
        public async Task SynchroniseWriteDBWithReadDB()
        {
            // Retrieve
            IEnumerable<CheckIn> writeModels = CheckInRepository.GetCheckIns();
            IEnumerable<CheckInReadModel> readModels = new List<CheckInReadModel>();
            Console.WriteLine("==== Start synchronisation ====");
            // Map all entities to read models
            foreach (CheckIn item in writeModels)
            {
                var convertedModel = item.MapToReadModel();
                readModels.Append(convertedModel);
            }
            // Insert into database.
            readModelRepository.BulkCreate(readModels);
            //
            Console.WriteLine("==== Synchronisation completed ====");
        }
    }
}
