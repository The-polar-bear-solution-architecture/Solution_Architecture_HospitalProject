using RabbitMQ.Messages.Messages;

namespace PatientService.Events.Patient
{
    public class PatientCreatedEvent : Event
    {
        public PatientCreatedEvent(string messageType) : base(messageType)
        {

        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string GeneralPractitionerEmail { get; set; }
    }
}
