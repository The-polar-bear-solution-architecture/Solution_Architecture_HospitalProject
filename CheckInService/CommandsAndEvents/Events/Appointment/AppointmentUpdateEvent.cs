using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events.Appointment
{
    public class AppointmentUpdateEvent: Event
    {
        public AppointmentUpdateEvent(string messageType) : base(messageType)
        {
        }

        public Guid CheckInSerialNr { get; set; }
        public Guid AppointmentSerialNr { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PhysicianSerialNr { get; set; }
    }
}
