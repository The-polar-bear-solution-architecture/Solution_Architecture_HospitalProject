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
    }
}
