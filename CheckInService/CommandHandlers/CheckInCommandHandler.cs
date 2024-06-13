using CheckinService.Model;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.CommandsAndEvents.Events;
using CheckInService.Mapper;
using CheckInService.Models;
using CheckInService.Models.DTO;
using CheckInService.Repositories;
using EventStore.Client;
using RabbitMQ.Messages.Interfaces;
using RabbitMQ.Messages.Mapper;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandHandlers
{
    public class CheckInCommandHandler
    {
        private readonly AppointmentRepository appointmentRepository;
        private readonly CheckInRepository checkInRepository;
        private readonly PatientRepo patientRepo;
        private readonly PhysicianRepo physicianRepo;
        private readonly IPublisher publisher;
        private readonly EventStoreClient eventStore;
        private readonly string RouterKeyLocator;

        public CheckInCommandHandler(
            AppointmentRepository appointmentRepository,
            CheckInRepository checkInRepository, 
            PatientRepo patientRepo, 
            PhysicianRepo physicianRepo,
            IPublisher publisher,
            EventStoreClient eventStore) {
            this.appointmentRepository = appointmentRepository;
            this.checkInRepository = checkInRepository;
            this.patientRepo = patientRepo;
            this.physicianRepo = physicianRepo;
            this.publisher = publisher;
            this.eventStore = eventStore;
            RouterKeyLocator = "Notifications";
        }

        public async Task<CheckIn> RegisterCheckin(CreateCheckInCommandDTO command)
        {
            // RegisterCheckin and CreateCheckInCommandDTO function as the command within CQRS.
            RegisterCheckin registerCheckinCommand = command.MapToRegister();
            // Converts DTO/Command to an proper domain entity, with the status AWAIT.
            CheckIn checkIn = registerCheckinCommand.MapToRegister();

            var patient = patientRepo.Get(checkIn.Appointment.Patient.Id);
            var physician = physicianRepo.Get(checkIn.Appointment.Physician.Id);

            if(patient != null)
            {
                patientRepo.Put(checkIn.Appointment.Patient);
                checkIn.Appointment.Patient = patient;
            }

            if(physician != null)
            {
                physicianRepo.Put(checkIn.Appointment.Physician);
                checkIn.Appointment.Physician = physician;
            }

            checkInRepository.Post(checkIn);

            // Store event within event source
            byte[] byteData = registerCheckinCommand.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), registerCheckinCommand.MessageType, byteData);
            await eventStore.AppendToStreamAsync(nameof(CheckIn), StreamState.Any, [eventData]);

            // Send event to Notification service
            var checkInEvent = registerCheckinCommand.MapCheckinRegistered();

            return checkIn;
        }

        // Change to noshow
        public async Task<CheckIn?> ChangeToNoShow(NoShowCheckIn command) {

            // Validate if checkin even exists.
            CheckIn? checkIn = checkInRepository.Get(command.CheckInId);
            if (checkIn == null)
            {
                return null;
            }

            // Change status according to command.
            checkIn.Status = command.Status;

            // Update check in.
            checkInRepository.Put(checkIn);

            // Add event to event source, for event sourcing
            Console.WriteLine("Add no show to the event source.");
            byte[] byteData = command.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), command.MessageType, byteData);
            await eventStore.AppendToStreamAsync(nameof(CheckIn), StreamState.Any, [eventData]);

            return checkIn;
        }

        // Change to noshow
        public async Task<CheckIn?> ChangeToPresent(PresentCheckin command)
        {
            // Validate if checkin even exists.
            CheckIn? checkIn = checkInRepository.Get(command.CheckInId);
            if (checkIn == null)
            {
                return null;
            }

            // Change status according to command.
            checkIn.Status = command.Status;

            // Update check in.
            checkInRepository.Put(checkIn);
            // Add event to event source, for event sourcing
            // Add guid, date of event, Operation: POST or PUT, ClassType, all the data.
            Console.WriteLine("Add no show to the event source.");
            // Fill in event source
            byte[] byteData = command.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), command.MessageType, byteData);
            await eventStore.AppendToStreamAsync(nameof(CheckIn), StreamState.Any, [eventData]);

            // Send notification to physician.
            Event checkInEvent = checkIn.MapToPatientIsPresent();
            await publisher.SendMessage(checkInEvent.MessageType, checkInEvent, RouterKeyLocator);

            return checkIn;
        }


    }
}
