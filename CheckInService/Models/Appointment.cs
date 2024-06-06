using CheckinService.Model;

namespace CheckInService.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Patient Patient { get; set; }
        public Physician Physician { get; set; }
    }
}
