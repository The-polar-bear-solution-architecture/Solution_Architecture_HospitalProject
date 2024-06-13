using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events
{
    public class PatientHasCheckedIn : Event
    {
        public int CheckInId { get; init; }
        public Status Status { get; init; } = Status.PRESENT;

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
