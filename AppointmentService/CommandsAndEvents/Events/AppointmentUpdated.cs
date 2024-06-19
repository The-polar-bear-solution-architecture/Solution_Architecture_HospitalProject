using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace AppointmentService.CommandsAndEvents.Events
{
    public class AppointmentUpdated  : Event
    {

        [Required]
        public Guid AppointmentId { get; set; }
        public string ApointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public Guid PhysicianId { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }


        public string PhysicianEmail { get; set; }
        public AppointmentUpdated() { }

        public AppointmentUpdated(Guid messageId) : base(messageId) { }

        public AppointmentUpdated(string messageType) : base(messageType) { }

        public AppointmentUpdated(Guid messageId, string messageType) : base(messageId, messageType) { }
    }
}
