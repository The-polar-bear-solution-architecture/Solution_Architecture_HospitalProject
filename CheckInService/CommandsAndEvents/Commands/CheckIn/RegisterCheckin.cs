using CheckinService.Model;
using CheckInService.Models;
using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.CommandsAndEvents.Commands.CheckIn
{
    public class RegisterCheckin : Command
    {
        public int CheckInId { get; init; }
        public Guid CheckInSerialNr { get; init; } = Guid.NewGuid();
        public Status Status { get; init; } = Status.AWAIT;

        // Alleen Gebruikt.
        [Required]
        public int AppointmentId { get; init; }
        public Guid AppointmentGuid { get; init; }
        public string ApointmentName { get; init; }
        public DateTime AppointmentDate { get; init; }

        [Required]
        public int PatientId { get; init; }
        public Guid PatientGuid { get; init; }
        public string PatientFirstName { get; init; }
        public string PatientLastName { get; init; }

        [Required]
        public int PhysicianId { get; init; }
        public Guid PhysicianGuid { get; init; }
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
