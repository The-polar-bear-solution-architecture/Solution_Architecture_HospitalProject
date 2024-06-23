using CheckInService.CommandHandlers;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.CommandsAndEvents.Commands.Patient;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.CommandsAndEvents.Events.Patient;
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
        private readonly PatientCommandHandler patientCommandHandler;

        public CheckInPipeline(
            EventStoreClient client, 
            EventStoreRepository eventStoreRepository,
            CheckInContextDB checkInContext, 
            ReadModelRepository readModelRepository, 
            CheckInCommandHandler checkInCommandHandler,
            CheckInRepository checkInRepository,
            PatientCommandHandler patientCommandHandler)
        {
            this.client = client;
            this.eventStoreRepository = eventStoreRepository;
            this.checkInContext = checkInContext;
            this.readModelRepository = readModelRepository;
            this.checkInCommandHandler = checkInCommandHandler;
            this.CheckInRepository = checkInRepository;
            this.patientCommandHandler = patientCommandHandler;
        }

        // Transports data from Event source to WriteDB and ReadDB.
        public async Task ReplayDataPipeline()
        {
            Console.WriteLine("==== Run pipeline ====");
            // Extract all data from event source.
            List<Message> list = new List<Message>();
            var events = await eventStoreRepository.GetFromCollection(nameof(CheckIn));
            // Delete all data from write database.

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
                        break;
                    case nameof(CheckInPresentEvent):
                        entity_event = data.Deserialize<PresentCheckin>();
                        var presentCheckIn = data.Deserialize<CheckInUpdateCommand>();
                        // Write to write db.
                        await checkInCommandHandler.ChangeToPresent((PresentCheckin)entity_event);
                        break;
                    case nameof(CheckInRegistrationEvent):
                        RegisterCheckin registerCommand = data.Deserialize<RegisterCheckin>();
                        CheckInReadModel readModel = data.Deserialize<CheckInReadModel>();
                        // Write to write db.
                        await checkInCommandHandler.RegisterCheckin(registerCommand);
                        break;
                    case nameof(AppointmentDeleteEvent):
                        var delete_command = data.Deserialize<AppointmentDeleteCommand>();
                        // Write to write db.
                        await checkInCommandHandler.DeleteAppointment(delete_command);
                        break;
                    case nameof(AppointmentUpdateEvent):
                        var update_command = data.Deserialize<AppointmentUpdateCommand>();
                        // Write to write db.
                        await checkInCommandHandler.UpdateAppointment(update_command);
                        break;
                    case nameof(PatientCreatedEvent):
                        var patient_create = data.Deserialize<PatientCreate>();
                        patientCommandHandler.RegisterPatient(patient_create);
                        break;
                    case nameof(PatientChangeEvent):
                        var patient_update = data.Deserialize<PatientUpdate>();
                        patientCommandHandler.ChangePatient(patient_update);
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
            List<CheckInReadModel> readModels = new List<CheckInReadModel>();
            Console.WriteLine("==== Start synchronisation ====");
            // Map all entities to read models
            foreach (CheckIn item in writeModels)
            {
                var convertedModel = item.MapToReadModel();
                readModels.Add(convertedModel);
            }
            // Insert into database.
            readModelRepository.BulkCreate(readModels);
            //
            Console.WriteLine("==== Synchronisation completed ====");
        }
    }
}
