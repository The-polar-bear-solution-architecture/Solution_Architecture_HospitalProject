using CheckinService.Model;
using CheckInService.Models;
using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.CommandsAndEvents.Events
{
    public class CheckInRegistrationEvent : Event
    {
        public int CheckInId { get; init; }
        public string CheckInSerialNr { get; init; }
        public Status Status { get; init; } = Status.AWAIT;

        [Required]
        public int AppointmentId { get; init; }
        public string ApointmentName { get; init; }
        public DateTime AppointmentDate { get; init; }

        [Required]
        public int PatientId { get; init; }
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

        public CheckInRegistrationEvent(): base(Guid.NewGuid(), nameof(CheckInRegistrationEvent))
        {
        }

        public CheckInRegistrationEvent(Guid messageId) : base(messageId)
        {
        }

        public CheckInRegistrationEvent(string messageType) : base(messageType)
        {
        }

        public CheckInRegistrationEvent(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
