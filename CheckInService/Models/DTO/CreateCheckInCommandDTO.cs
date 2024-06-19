using CheckinService.Model;
using CheckInService.Models;
using System.ComponentModel.DataAnnotations;

namespace CheckInService.Models.DTO
{
    public class CreateCheckInCommandDTO
    {
        public int Id { get; set; }
        public Status Status { get; set; } = Status.AWAIT;

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public Guid AppointmentGuid { get; set; } = Guid.NewGuid();

        public string ApointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Guid PatientGuid { get; set; } = Guid.NewGuid();
        public Guid PhysicianGuid { get; set; } = Guid.NewGuid();

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
    }
}
