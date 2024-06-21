using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace PatientService.Events.GeneralPractitioner
{
    public class GPCreatedEvent : Event
    {
        public GPCreatedEvent(string messageType) : base(messageType)
        {

        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
