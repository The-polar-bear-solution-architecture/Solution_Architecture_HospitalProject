using CheckinService.Model;
using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Commands
{
    public class ChangeCheckInCommand : Command
    {
        public int CheckInId { get; init; }
        public string CheckInSerialNr { get; init; }
        public Status Status { get; init; }

        public ChangeCheckInCommand(Guid messageId) : base(messageId)
        {
        }

        public ChangeCheckInCommand(string messageType) : base(messageType)
        {
        }

        public ChangeCheckInCommand(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }

        public ChangeCheckInCommand()
        {
        }
    }
}
