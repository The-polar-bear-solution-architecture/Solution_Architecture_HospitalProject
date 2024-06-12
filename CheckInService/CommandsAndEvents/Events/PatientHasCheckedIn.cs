using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events
{
    public class PatientHasCheckedIn : Event
    {
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public string PhysicianEmail { get; set; }

        public PatientHasCheckedIn()
        {
        }

        public PatientHasCheckedIn(Guid messageId) : base(messageId)
        {
        }

        public PatientHasCheckedIn(string messageType) : base(messageType)
        {
        }

        public PatientHasCheckedIn(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
