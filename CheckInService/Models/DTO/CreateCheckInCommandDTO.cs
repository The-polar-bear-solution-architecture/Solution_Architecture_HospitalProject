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
        public Guid AppointmentId { get; set; }
        public string ApointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }      

        public Guid PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        
        public Guid PhysicianId { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public string PhysicianEmail { get; set; }
    }
}
