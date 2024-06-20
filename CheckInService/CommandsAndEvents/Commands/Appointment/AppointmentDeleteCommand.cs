namespace CheckInService.CommandsAndEvents.Commands.Appointment
{
    public class AppointmentDeleteCommand
    {
        public Guid CheckInSerialNr { get; set; }
        public Guid AppointmentSerialNr { get; set; }
    }
}
