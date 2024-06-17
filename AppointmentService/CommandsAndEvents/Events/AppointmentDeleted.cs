using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace AppointmentService.CommandsAndEvents.Events
{
    public class AppointmentDeleted : Event
    {
        [Required]
        public int AppointmentId { get; set; }

        public AppointmentDeleted() { }

        public AppointmentDeleted(Guid messageId) : base(messageId) { }

        public AppointmentDeleted(string messageType) : base(messageType) { }

        public AppointmentDeleted(Guid messageId, string messageType) : base(messageId, messageType) { }
    }
}
