using CheckinService.Model;
using CheckInService.Models;
using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.CommandsAndEvents.Commands
{
    public class RegisterCheckin: Command
    {
        public int CheckInId { get; set; }
        public Status Status { get; set; } = Status.AWAIT;

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

        public RegisterCheckin()
        {
        }

        public RegisterCheckin(Guid messageId) : base(messageId)
        {
        }

        public RegisterCheckin(string messageType) : base(messageType)
        {
        }

        public RegisterCheckin(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
