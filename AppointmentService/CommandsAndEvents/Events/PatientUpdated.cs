using AppointmentService.Domain;
using RabbitMQ.Messages.Messages;

namespace AppointmentService.CommandsAndEvents.Events
{
    public class PatientUpdated : Event
    {

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public GeneralPractitioner GeneralPractitioner { get; set; }

        public PatientUpdated() { }

        public PatientUpdated(Guid messageId) : base(messageId) { }

        public PatientUpdated(string messageType) : base(messageType) { }

        public PatientUpdated(Guid messageId, string messageType) : base(messageId, messageType) { }
    }

    
}
