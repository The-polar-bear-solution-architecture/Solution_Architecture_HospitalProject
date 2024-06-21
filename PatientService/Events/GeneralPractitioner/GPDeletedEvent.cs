using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace PatientService.Events.GeneralPractitioner
{
    public class GPDeletedEvent : Event
    {
        public GPDeletedEvent(string messageType) : base(messageType)
        {

        }

        public Guid Id { get; set; }
    }
}
