using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events.Patient
{
    public class PatientCreatedEvent: Event
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public Guid GPId { get; set; }
        public string GPFirstName { get; set; }
        public string GPLastName { get; set; }

        public PatientCreatedEvent(Guid messageId) : base(messageId)
        {
        }

        public PatientCreatedEvent(string messageType) : base(messageType)
        {
        }
    }
}
