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

            var patient = patientRepo.Get(registerCheckinCommand.PatientId);
            var physician = physicianRepo.Get(registerCheckinCommand.PhysicianId);

            Console.WriteLine(patient);
            if(patient != null)
            {
                Console.WriteLine("Patient is updated");
                // Overwrite the current patients info to local patient
                Patient tempPatient = checkIn.Appointment.Patient;
                patient.FirstName = tempPatient.FirstName;
                patient.LastName = tempPatient.LastName;
                patientRepo.Put(patient);
                checkIn.Appointment.Patient = patient;
            }

            if(physician != null)
            {
                Console.WriteLine("Physician is updated");
                Physician tempPhysician = checkIn.Appointment.Physician;
                physician.FirstName = tempPhysician.FirstName;
                physician.LastName = tempPhysician.LastName;
                physician.Email = tempPhysician.Email;
                physicianRepo.Put(physician);
                //
                checkIn.Appointment.Physician = physician;
            }

            checkInRepository.Post(checkIn);

            // Store event within event source
            var eventBody  = registerCheckinCommand.MapCheckinRegistered(checkIn.Id);
            byte[] byteData = eventBody.Serialize();
            var eventData = new EventData(Uuid.NewUuid(), registerCheckinCommand.MessageType, byteData);
            await eventStore.AppendToStreamAsync(nameof(CheckIn), StreamState.Any, [eventData]);

            // Send event to Notification service
            Console.WriteLine("Important: Send to notification service so user will receive message evening before Appointment");

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
            Console.WriteLine("Start checkin to present");
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

            // Send notification to physician.
            Console.WriteLine(checkIn);

            PatientHasCheckedIn checkInEvent = checkIn.MapToPatientIsPresent();
            await publisher.SendMessage(checkInEvent.MessageType, checkInEvent, RouterKeyLocator);

            return checkIn;
        }


    }
}
