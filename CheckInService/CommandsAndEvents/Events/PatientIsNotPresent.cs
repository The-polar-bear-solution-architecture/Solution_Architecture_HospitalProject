using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events
{
    public class PatientIsNotPresent : Event
    {
        public int CheckInId { get; init; }
        public string CheckInSerialNr { get; init; }
        public Status Status { get; init; } = Status.NOSHOW;

        public PatientIsNotPresent()
        {
        }

        public PatientIsNotPresent(Guid messageId) : base(messageId)
        {
        }

        public PatientIsNotPresent(string messageType) : base(messageType)
        {
        }

        public PatientIsNotPresent(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
