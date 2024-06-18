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
        private readonly EventStoreRepository eventStoreRepository;
        private readonly string RouterKeyLocator;

        public CheckInCommandHandler(
            AppointmentRepository appointmentRepository,
            CheckInRepository checkInRepository, 
            PatientRepo patientRepo, 
            PhysicianRepo physicianRepo,
            IPublisher publisher,
            EventStoreClient eventStore,
            EventStoreRepository eventStoreRepository) {
            this.appointmentRepository = appointmentRepository;
            this.checkInRepository = checkInRepository;
            this.patientRepo = patientRepo;
            this.physicianRepo = physicianRepo;
            this.publisher = publisher;
            this.eventStore = eventStore;
            this.eventStoreRepository = eventStoreRepository;
            RouterKeyLocator = "Notifications";
        }

        public async Task<CheckIn> RegisterCheckin(RegisterCheckin command)
        {
            // Converts DTO/Command to an proper domain entity, with the status AWAIT.
            CheckIn checkIn = command.MapToRegister();

            var patient = patientRepo.Get(command.PatientGuid);
            var physician = physicianRepo.Get(command.PhysicianGuid);

            if(patient != null)
            {
                // Overwrite the current patients info to local patient
                patient.FirstName = command.PatientFirstName;
                patient.LastName = command.PatientLastName;
                patientRepo.Put(patient);
                checkIn.Appointment.Patient = patient;
            }

            if(physician != null)
            {
                physician.FirstName = command.PhysicianFirstName;
                physician.LastName = command.PhysicianLastName;
                physician.Email = command.PhysicianEmail;

                physicianRepo.Put(physician);
                checkIn.Appointment.Physician = physician;
            }

            checkInRepository.Post(checkIn);

            // Checkin registration event.
            var RegisterEvent  = command.MapCheckinRegistered(checkIn.Id, checkIn.SerialNr, checkIn.Appointment.Id);
            await eventStoreRepository.StoreMessage(RegisterEvent.MessageType, RegisterEvent);
            Console.WriteLine($"Type of event is {RegisterEvent.MessageType}");
            // Send event to Notification service
            Console.WriteLine("Important: Send to notification service so user will receive message evening before Appointment");

            return checkIn;
        }

        // Change to noshow
        public async Task<CheckIn?> ChangeToNoShow(NoShowCheckIn command) {

            // Validate if checkin even exists.
            CheckIn? checkIn = checkInRepository.Get(command.CheckInSerialNr);
            if (checkIn == null)
            {
                return null;
            }

            // Change status according to command.
            checkIn.Status = command.Status;

            // Update check in.
            checkInRepository.Put(checkIn);

            // No show event.
            var NoShowEvent = checkIn.MapPatientNoShow();

            // Add event to event store.
            await eventStoreRepository.StoreMessage(NoShowEvent.MessageType, NoShowEvent);

            return checkIn;
        }

        // Change to noshow
        public async Task<CheckIn?> ChangeToPresent(PresentCheckin command)
        {
            Console.WriteLine("Start checkin to present");
            // Validate if checkin even exists.
            CheckIn? checkIn = checkInRepository.Get(command.CheckInSerialNr);
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

            // ZET HIER EVENT CONVERSION NEER.
            // Checkin present event.
            var PresentEvent = checkIn.MapToPatientIsPresent();

            // Add event to event store.
            await eventStoreRepository.StoreMessage(PresentEvent.MessageType, PresentEvent);

            // Send notification to physician.
            Console.WriteLine(checkIn);

            // Convert to event.
            CheckInPresentEvent checkInEvent = checkIn.MapToPatientIsPresent();
            await publisher.SendMessage(checkInEvent.MessageType, checkInEvent, RouterKeyLocator);

            return checkIn;
        }


    }
}
