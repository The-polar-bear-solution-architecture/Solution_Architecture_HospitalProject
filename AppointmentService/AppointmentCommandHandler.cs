using AppointmentService.CommandsAndEvents.Events;
using AppointmentService.Domain;
using AppointmentService.DomainServices;
using RabbitMQ.Messages.Interfaces;

namespace AppointmentService
{
    public class AppointmentCommandHandler
    {

        private readonly IAppointmentRepository _repository;
        private readonly IPatientRepository _patientRepository;
        private readonly IPhysicianRepository _PhysicianRepository;
        private readonly IPublisher publisher;
        private readonly string RouterKeyLocator;

        public AppointmentCommandHandler (IAppointmentRepository repository, IPatientRepository patientRepository, IPhysicianRepository physicianRepository, IPublisher publisher)
        {
            _repository = repository;
            _patientRepository = patientRepository;
            _PhysicianRepository = physicianRepository;
            this.publisher = publisher;
            RouterKeyLocator = "Appointments_Checkin";

        }

        public void AppointmentCreated(AppointmentCreated appointmentCreated)
        {
            this.publisher.SendMessage(appointmentCreated.MessageType, appointmentCreated, RouterKeyLocator);
        }

        public void AppointmentDeleted(AppointmentDeleted appointmentDeleted)
        {
            this.publisher.SendMessage(appointmentDeleted.MessageType, appointmentDeleted, RouterKeyLocator);

        }
        public void AppointmentUpdated(AppointmentUpdated appointmentUpdated)
        {
            this.publisher.SendMessage(appointmentUpdated.MessageType, appointmentUpdated, RouterKeyLocator);

        }
    }
}
