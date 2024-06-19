using CheckinService.Model;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.Models
{
    public class CheckInReadModel
    {
        [Key]
        public Guid CheckInSerialNr { get; init; }
        public Status Status { get; set; } = Status.AWAIT;

        [Required]
        public Guid AppointmentGuid { get; init; }
        public string ApointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }

        [Required]
        public Guid PatientGuid { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        [Required]
        public Guid PhysicianGuid { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }

        [Required]
        public string PhysicianEmail { get; set; }
    }
}
