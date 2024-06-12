using RabbitMQ.Messages.Messages;

namespace CheckInService.CommandsAndEvents.Events
{
    public class PatientIsNotPresent : Event
    {
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
