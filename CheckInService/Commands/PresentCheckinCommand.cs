using RabbitMQ.Messages.Messages;

namespace CheckInService.Commands
{
    public class PresentCheckinCommand : Command
    {
        public PresentCheckinCommand(Guid messageId) : base(messageId)
        {
        }

        public PresentCheckinCommand(string messageType) : base(messageType)
        {
        }

        public PresentCheckinCommand(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
