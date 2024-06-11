using RabbitMQ.Messages.Messages;

namespace CheckInService.Commands
{
    public class NoShowCheckInCommand : Command
    {
        public NoShowCheckInCommand(Guid messageId) : base(messageId)
        {
        }

        public NoShowCheckInCommand(string messageType) : base(messageType)
        {
        }

        public NoShowCheckInCommand(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
