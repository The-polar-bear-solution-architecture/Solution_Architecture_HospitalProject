using AppointmentService.Domain;
using RabbitMQ.Messages.Messages;

namespace AppointmentService.CommandsAndEvents.Events
{
    public class PatientCreated : Event
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public GeneralPractitioner GeneralPractitioner { get; set; }

        public PatientCreated() { }

        public PatientCreated(Guid messageId) : base(messageId) { }

        public PatientCreated(string messageType) : base(messageType) { }

        public PatientCreated(Guid messageId, string messageType) : base(messageId, messageType) { }

    }
}
