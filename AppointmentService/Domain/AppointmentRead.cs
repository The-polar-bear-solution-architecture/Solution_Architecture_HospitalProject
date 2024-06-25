using System.ComponentModel.DataAnnotations;

namespace AppointmentService.Domain
{
    public class AppointmentRead
    {
        [Key]
        public Guid AppointmentId { get; set; }
        public Guid PhysicianId { get; set; }
        public Guid GPId { get; set; }
        public Guid PatientId { get; set; }
        public Guid? PreviousAppointmentId { get; set; }
        public string Name { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public string PhysicianEmail { get; set; }
        public Role PhysicianRole { get; set; }
        public string GPFirstName { get; set; }
        public string GPFlastName { get; set; }
        public string GPEmail { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string PatientPhoneNumber { get; set; }


    }
}
