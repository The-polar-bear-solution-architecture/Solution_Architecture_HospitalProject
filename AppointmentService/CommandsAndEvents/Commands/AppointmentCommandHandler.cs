using AppointmentService.CommandsAndEvents.Events;
using AppointmentService.Domain;
using AppointmentService.DomainServices;
using RabbitMQ.Messages.Interfaces;

namespace AppointmentService.CommandsAndEvents.Commands
{
    public class AppointmentCommandHandler
    {

        private readonly IAppointmentRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPhysicianRepository _PhysicianRepository;
        private readonly IPublisher publisher;
        private readonly string RouterKeyLocator;

        public AppointmentCommandHandler(IAppointmentRepository repository, IPatientRepository patientRepository, IPhysicianRepository physicianRepository, IPublisher publisher)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _PhysicianRepository = physicianRepository;
            this.publisher = publisher;
            RouterKeyLocator = "Appointments_Checkin";

        }

        public async Task AppointmentCreated(AppointmentCreated appointmentCreated)
        {
            await publisher.SendMessage(appointmentCreated.MessageType, appointmentCreated, RouterKeyLocator);
        }

        public async Task AppointmentDeleted(AppointmentDeleted appointmentDeleted)
        {
            await publisher.SendMessage(appointmentDeleted.MessageType, appointmentDeleted, RouterKeyLocator);

        }
        public async Task AppointmentUpdated(AppointmentUpdated appointmentUpdated)
        {
            await publisher.SendMessage(appointmentUpdated.MessageType, appointmentUpdated, RouterKeyLocator);

        }
    }
}
