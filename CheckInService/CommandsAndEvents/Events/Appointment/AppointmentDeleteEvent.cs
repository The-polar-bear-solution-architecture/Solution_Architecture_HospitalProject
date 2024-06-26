using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events.Appointment
{
    public class AppointmentDeleteEvent: Event
    {
        public Guid CheckInSerialNr { get; set; }
        public Guid AppointmentSerialNr { get; set; }

        public AppointmentDeleteEvent(string messageType): base(messageType) { }
    }
}
