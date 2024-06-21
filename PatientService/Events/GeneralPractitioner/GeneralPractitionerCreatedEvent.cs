using RabbitMQ.Messages.Messages;

namespace PatientService.Events.GeneralPractitioner
{
    public class GeneralPractitionerCreatedEvent : Event
    {
        public GeneralPractitionerCreatedEvent(string messageType) : base(messageType)
        {
        }
    }
}
