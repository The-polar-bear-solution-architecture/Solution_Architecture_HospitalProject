namespace CheckInService.Models.DTO
{
    public class UpdateCheckInDTO
    {
        public Guid AppointmentId { get; set; }
        public string ApointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }

        public Guid PhysicianId { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }

        
        public Guid PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
    }
}
