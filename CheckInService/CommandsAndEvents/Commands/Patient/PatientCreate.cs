using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Commands.Patient
{
    public class PatientCreate: Command
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public Guid GPId { get; set; }
        public string GPFirstName { get; set; }
        public string GPLastName
        {
            get; set;
        }

        public PatientCreate(Guid messageId) : base(messageId)
        {
        }

        public PatientCreate(string messageType) : base(messageType)
        {
        }
    }
}
