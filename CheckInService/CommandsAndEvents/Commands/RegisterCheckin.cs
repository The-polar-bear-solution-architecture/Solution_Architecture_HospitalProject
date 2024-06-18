using CheckinService.Model;
using CheckInService.Models;
using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.CommandsAndEvents.Commands
{
    public class RegisterCheckin: Command
    {
        public int CheckInId { get; init; }
        public string CheckinSerialNr { get; init; } = Guid.NewGuid().ToString();
        public Status Status { get; init; } = Status.AWAIT;

        // Alleen Gebruikt.
        public string AppointmentGuid { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public int AppointmentId { get; init; }
        public string ApointmentName { get; init; }
        public DateTime AppointmentDate { get; init; }

        [Required]
        public int PatientId { get; init; }

        // public string PatientGuid { get; init; } = Guid.NewGuid().ToString();
        // public string PhysicianGuid { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string PatientFirstName { get; init; }
        [Required]
        public string PatientLastName { get; init; }

        [Required]
        public int PhysicianId { get; init; }
        public string PhysicianFirstName { get; init; }
        public string PhysicianLastName { get; init; }

        [Required]
        public string PhysicianEmail { get; init; }

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
