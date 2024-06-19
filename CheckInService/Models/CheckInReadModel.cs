using CheckinService.Model;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.Models
{
    public class CheckInReadModel
    {
        public int CheckInId { get; init; }
        public Guid CheckInSerialNr { get; init; }
        public Status Status { get; init; } = Status.AWAIT;

        [Required]
        public Guid AppointmentGuid { get; init; }
        public int AppointmentId { get; init; }
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
    }
}
