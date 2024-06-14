using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Commands
{
    public class NoShowCheckIn : Command
    {
        public int CheckInId { get; init; }
        public string CheckInSerialNr { get; init; }
        public Status Status { get; init; } = Status.NOSHOW;

        public NoShowCheckIn(Guid messageId) : base(messageId)
        {
        }

        public NoShowCheckIn(string messageType) : base(messageType)
        {
        }

        public NoShowCheckIn(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }

        public NoShowCheckIn(): base(Guid.NewGuid(), nameof(NoShowCheckIn)) { }
        {

        }
    }
}
