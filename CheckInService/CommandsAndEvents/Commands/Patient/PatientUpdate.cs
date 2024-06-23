using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Commands.Patient
{
    public class PatientUpdate: Command
    {
        public PatientUpdate(string messageType) : base(messageType)
        {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
