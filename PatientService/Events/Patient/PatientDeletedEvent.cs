using RabbitMQ.Messages.Messages;

namespace PatientService.Events.Patient
{
    public class PatientDeletedEvent : Event
    {
        public PatientDeletedEvent(string messageType) : base(messageType)
        {

        }

        public Guid Id { get; set; }
    }
}
