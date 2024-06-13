using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events
{
    public class PatientHasCheckedIn : Event
    {
        public int CheckInId { get; init; }
        public Status Status { get; init; } = Status.PRESENT;

        public string PatientFirstName { get; init; }
        public string PatientLastName { get; init; }

        public string PhysicianFirstName { get; init; }
        public string PhysicianLastName { get; init; }
        public string PhysicianEmail { get; init; }

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
