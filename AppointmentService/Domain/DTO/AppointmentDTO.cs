namespace AppointmentService.Domain.DTO
{
    public class AppointmentDTO
    {

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid PhysicianId { get; set; }
        public Guid PatientId { get; set; }
        public Guid? PreviousAppointmentId { get; set; }
    }
}
