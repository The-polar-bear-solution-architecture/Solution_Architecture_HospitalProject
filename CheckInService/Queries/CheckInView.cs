using CheckinService.Model;

namespace CheckInService.Queries
{
    public class CheckInView
    {
        public int Id { get; set; }
        public Status Status { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public string PhysicianEmail { get;}
    }
}
