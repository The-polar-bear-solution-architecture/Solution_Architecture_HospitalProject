using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events.Patient
{
    public class PatientChangeEvent: Event
    {
        public PatientChangeEvent(string messageType) : base(messageType)
        {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
