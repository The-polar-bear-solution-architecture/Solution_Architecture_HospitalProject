using CheckinService.Model;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.CommandsAndEvents.Events;
using CheckInService.Mapper;
using CheckInService.Models;
using CheckInService.Repositories;
using RabbitMQ.Messages.Interfaces;
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

        private readonly string RouterKeyLocator;

        public CheckInCommandHandler(
            AppointmentRepository appointmentRepository,
            CheckInRepository checkInRepository, 
            PatientRepo patientRepo, 
            PhysicianRepo physicianRepo,
            IPublisher publisher) {
            this.appointmentRepository = appointmentRepository;
            this.checkInRepository = checkInRepository;
            this.patientRepo = patientRepo;
            this.physicianRepo = physicianRepo;
            this.publisher = publisher;

            RouterKeyLocator = "Notifications";
        }

        public CheckIn RegisterCheckin(RegisterCheckin command)
        {
            return null;
        }

        // Change to noshow
        public CheckIn? ChangeToNoShow(NoShowCheckIn command) {

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

            // Send notification to physician.
            Event checkInEvent = checkIn.MapToPatientIsPresent();
            await publisher.SendMessage(checkInEvent.MessageType, checkInEvent, RouterKeyLocator);

            return checkIn;
        }


    }
}
