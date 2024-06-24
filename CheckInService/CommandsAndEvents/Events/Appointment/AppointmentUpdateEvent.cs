using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events.Appointment
{
    public class AppointmentUpdateEvent: Event
    {
        public AppointmentUpdateEvent(string messageType) : base(messageType)
        {
        }
        public Guid AppointmentSerialNr { get; set; }
        public string AppointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PhysicianSerialNr { get; set; }
        public string PhysicianFirstName { get; init; }
        public string PhysicianLastName { get; init; }
        public string PhysicianEmail { get; init; }
    }
}
