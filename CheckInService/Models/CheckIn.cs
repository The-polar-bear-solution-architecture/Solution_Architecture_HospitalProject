using CheckinService.Model;

namespace CheckInService.Models
{
    public class CheckIn
    {
        public int Id { get; set; }
        public string SerialNr { get; set; } = Guid.NewGuid().ToString();
        public Status Status { get; set; } = Status.AWAIT;
        
        public Appointment Appointment { get; set; }
    }
}
