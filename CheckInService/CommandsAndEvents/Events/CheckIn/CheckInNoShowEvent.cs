using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events.CheckIn
{
    public class CheckInNoShowEvent : Event
    {
        public Guid CheckInSerialNr { get; init; }
        public Guid AppointmentSerialNr { get; init; }
        public Status Status { get; init; } = Status.NOSHOW;

        public CheckInNoShowEvent() : base(Guid.NewGuid(), nameof(CheckInPresentEvent))
        {
        }

        public CheckInNoShowEvent(Guid messageId) : base(messageId)
        {
        }

        public CheckInNoShowEvent(string messageType) : base(messageType)
        {
        }

        public CheckInNoShowEvent(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
