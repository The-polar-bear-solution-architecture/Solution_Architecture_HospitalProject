using CheckinService.Model;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Events.CheckIn;
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
        

        public CheckInCommandHandler(
            AppointmentRepository appointmentRepository,
            CheckInRepository checkInRepository, 
            PatientRepo patientRepo, 
            PhysicianRepo physicianRepo) {
            this.appointmentRepository = appointmentRepository;
            this.checkInRepository = checkInRepository;
            this.patientRepo = patientRepo;
            this.physicianRepo = physicianRepo;
            
        }

        public async Task<CheckInRegistrationEvent> RegisterCheckin(RegisterCheckin command)
        {
            // Converts DTO/Command to an proper domain entity, with the status AWAIT.
            CheckIn checkIn = command.MapToRegister();

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

            if(physician != null)
            {
                physician.FirstName = command.PhysicianFirstName;
                physician.LastName = command.PhysicianLastName;
                physician.Email = command.PhysicianEmail;

                physicianRepo.Put(physician);
                checkIn.Appointment.Physician = physician;
            }

            if (ExistingAppointment != null)
            {
                checkIn.Appointment = ExistingAppointment;
            }

            checkInRepository.Post(checkIn);

            // Checkin registration event.
            CheckInRegistrationEvent RegisterEvent  = command.MapCheckinRegistered(checkIn.Id, checkIn.SerialNr, checkIn.Appointment.Id);

            return RegisterEvent;
        }

        // Change to noshow
        public async Task<CheckInNoShowEvent?> ChangeToNoShow(NoShowCheckIn command) {

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

            return NoShowEvent;
        }

        // Change to noshow
        public async Task<CheckInPresentEvent?> ChangeToPresent(PresentCheckin command)
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
            try
            {
                checkInRepository.Put(checkIn);
            }
            catch
            {
               
            }

            // Add event to event source, for event sourcing
            Console.WriteLine("Add no show to the event source.");

            // Checkin present event.
            var PresentEvent = checkIn.MapToPatientIsPresent();

           
            return PresentEvent;
        }

        public async Task<AppointmentUpdateEvent?> UpdateAppointment(AppointmentUpdateCommand appointmentUpdateCommand)
        {
            Appointment? appointment = appointmentRepository.Get(appointmentUpdateCommand.AppointmentSerialNr);
            if (appointment == null)
            {
                return null;
            }
            Console.WriteLine("Appointment is found");

            appointment.AppointmentDate = appointmentUpdateCommand.AppointmentDate;
            appointment.Name = appointmentUpdateCommand.AppointmentName;
            
            var retrieved_physician = physicianRepo.Get(appointmentUpdateCommand.PhysicianSerialNr);
            // Will only change physician of appointment if it exists or is a different one then currently assigned to the appointment.
            if (retrieved_physician != null && !retrieved_physician.PhysicianSerialNr.Equals(appointmentUpdateCommand.PhysicianSerialNr))
            {
                appointment.Physician = retrieved_physician;
            }
            // Update appointment
            appointmentRepository.Put(appointment);

            // Store event into event store database
            AppointmentUpdateEvent updateEvent = appointmentUpdateCommand.MapToUpdatedEvent(retrieved_physician);
            
            return updateEvent;
        }

        public async Task<AppointmentDeleteEvent?> DeleteAppointment(AppointmentDeleteCommand appointmentDeleteCommand)
        {
            Appointment? appointment = appointmentRepository.Get(appointmentDeleteCommand.AppointmentSerialNr);
            if (appointment == null)
            {
                return null;
            }

            // Delete appointment.
            appointmentRepository.Delete(appointmentDeleteCommand.AppointmentSerialNr);

            // Store event into event store database
            AppointmentDeleteEvent deleteEvent = appointmentDeleteCommand.MapToAppointmentDeleted();

            return deleteEvent;
        }
    }
}
