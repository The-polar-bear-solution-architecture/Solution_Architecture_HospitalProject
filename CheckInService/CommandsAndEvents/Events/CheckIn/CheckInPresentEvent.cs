using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events.CheckIn
{
    public class CheckInPresentEvent : Event
    {
        public int CheckInId { get; init; }
        public Guid CheckInSerialNr { get; init; }
        public Status Status { get; init; } = Status.PRESENT;

        public CheckInPresentEvent() : base(Guid.NewGuid(), nameof(CheckInPresentEvent))
        {
        }

        public CheckInPresentEvent(Guid messageId) : base(messageId)
        {
        }

        public CheckInPresentEvent(string messageType) : base(messageType)
        {
        }

        public CheckInPresentEvent(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
