using CheckinService.Model;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.CommandsAndEvents.Events;
using CheckInService.Mapper;
using CheckInService.Models;
using CheckInService.Repositories;
using EventStore.Client;
using RabbitMQ.Messages.Interfaces;

namespace CheckInService.CommandHandlers
{
    public class ReplayHandler
    {
        private readonly AppointmentRepository appointmentRepository;
        private readonly CheckInRepository checkInRepository;
        private readonly PatientRepo patientRepo;
        private readonly PhysicianRepo physicianRepo;
        private readonly EventStoreRepository eventStoreRepository;
        private readonly string RouterKeyLocator;

        public readonly IPublisher Publisher;

        public ReplayHandler(
            AppointmentRepository appointmentRepository,
            CheckInRepository checkInRepository,
            PatientRepo patientRepo,
            PhysicianRepo physicianRepo,
            IPublisher publisher,
            EventStoreRepository eventStoreRepository)
        {
            this.appointmentRepository = appointmentRepository;
            this.checkInRepository = checkInRepository;
            this.patientRepo = patientRepo;
            this.physicianRepo = physicianRepo;
            this.eventStoreRepository = eventStoreRepository;
            this.Publisher = publisher;
            RouterKeyLocator = "Notifications";
        }

        public async Task<CheckIn> RegisterCheckin(CheckInRegistrationEvent command)
        {
            // Converts DTO/Command to an proper domain entity, with the status AWAIT.
            CheckIn checkIn = new CheckIn()
            {
                Status = command.Status,
                SerialNr = command.CheckInSerialNr,
                Appointment = new Appointment()
                {
                    AppointmentSerialNr = command.AppointmentGuid,
                    AppointmentDate = command.AppointmentDate,
                    Name = command.ApointmentName,
                    Patient = new Patient()
                    {
                        PatientSerialNr = command.PatientGuid,
                        FirstName = command.PatientFirstName,
                        LastName = command.PatientLastName
                    },
                    Physician = new Physician()
                    {
                        PhysicianSerialNr = command.PhysicianGuid,
                        FirstName = command.PhysicianFirstName,
                        LastName = command.PhysicianLastName,
                        Email = command.PhysicianEmail
                    }
                },
            };

            var patient = patientRepo.Get(command.PatientGuid);
            var physician = physicianRepo.Get(command.PhysicianGuid);
            var ExistingAppointment = appointmentRepository.Get(command.AppointmentGuid);

            if (patient != null)
            {
                // Overwrite the current patients info to local patient
                patient.FirstName = command.PatientFirstName;
                patient.LastName = command.PatientLastName;
                patientRepo.Put(patient);
                checkIn.Appointment.Patient = patient;
            }

            if (physician != null)
            {
                physician.FirstName = command.PhysicianFirstName;
                physician.LastName = command.PhysicianLastName;
                physician.Email = command.PhysicianEmail;
                physicianRepo.Put(physician);
                checkIn.Appointment.Physician = physician;
            }

            if(ExistingAppointment != null)
            {
                checkIn.Appointment = ExistingAppointment;
            }

            checkInRepository.Post(checkIn);

            // Send event to Notification service
            Console.WriteLine("Checkin has been replayed.");

            return checkIn;
        }

        // Change to noshow
        public async Task<CheckIn?> ChangeToNoShow(CheckInNoShowEvent command)
        {
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

            return checkIn;
        }

        // Change to noshow
        public async Task<CheckIn?> ChangeToPresent(CheckInPresentEvent command)
        {
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

            return checkIn;
        }
    }
}
