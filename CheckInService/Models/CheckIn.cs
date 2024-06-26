using CheckinService.Model;

namespace CheckInService.Models
{
    public class CheckIn
    {
        public int Id { get; set; }
        public Guid SerialNr { get; set; } = Guid.NewGuid();
        public Status Status { get; set; } = Status.AWAIT;
        
        public Appointment Appointment { get; set; }
    }
}
