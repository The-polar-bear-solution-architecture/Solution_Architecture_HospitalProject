﻿using CheckinService.Model;
using CheckInService.Models;
using RabbitMQ.Messages.Messages;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.CommandsAndEvents.Commands
{
    public class RegisterCheckin: Command
    {
        public int CheckInId { get; init; }
        public Guid CheckinSerialNr { get; init; } = Guid.NewGuid();
        public Status Status { get; init; } = Status.AWAIT;

        // Alleen Gebruikt.
        public Guid AppointmentGuid { get; init; } = Guid.NewGuid();

        [Required]
        public int AppointmentId { get; init; }
        public string ApointmentName { get; init; }
        public DateTime AppointmentDate { get; init; }

        [Required]
        public int PatientId { get; init; }

        public Guid PatientGuid { get; init; } = Guid.NewGuid();
        public Guid PhysicianGuid { get; init; } = Guid.NewGuid();

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
