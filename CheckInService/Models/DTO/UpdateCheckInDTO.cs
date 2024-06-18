namespace CheckInService.Models.DTO
{
    public class UpdateCheckInDTO
    {
        public Guid AppointmentGuid { get; set; }
        public string ApointmentName { get; set; }
        public DateTime AppointmentDate { get; set; }

        public Guid PhysicianGuid { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }

        /* Not needed.
         * public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; } 
        */

    }
}
