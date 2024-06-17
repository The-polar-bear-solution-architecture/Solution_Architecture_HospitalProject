using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;


namespace AppointmentService.CommandsAndEvents.Events
{


    public class AppointmentCreated : Event
    {

        [Required]
        public int AppointmentId { get; set; }
        public string ApointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }

        [Required]
        public int PatientId { get; set; }
        [Required]
        public string PatientFirstName { get; set; }
        [Required]
        public string PatientLastName { get; set; }

        [Required]
        public int PhysicianId { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }

        [Required]
        public string PhysicianEmail { get; set; }
        public AppointmentCreated() { } 
        
        public AppointmentCreated(Guid messageId) : base(messageId) { }

        public AppointmentCreated(string messageType) : base(messageType) { }
        
        public AppointmentCreated(Guid messageId, string messageType) : base(messageId, messageType) { }


    }
}
