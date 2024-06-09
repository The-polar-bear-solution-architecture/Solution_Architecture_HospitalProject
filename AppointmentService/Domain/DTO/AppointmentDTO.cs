namespace AppointmentService.Domain.DTO
{
    public class AppointmentDTO
    {

        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int PhysicianId { get; set; }
        public int PatientId { get; set; }
        public int? PreviousAppointmentId { get; set; }
    }
}
