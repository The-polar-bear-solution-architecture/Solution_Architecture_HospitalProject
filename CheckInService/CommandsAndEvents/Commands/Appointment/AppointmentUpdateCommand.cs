namespace CheckInService.CommandsAndEvents.Commands.Appointment
{
    public class AppointmentUpdateCommand
    {
        public Guid AppointmentSerialNr { get; init; }
        public string AppointmentName { get; init; }
        public DateTime AppointmentDate { get; init; }
        public Guid PhysicianSerialNr { get; init; }
        public string PhysicianFirstName { get; init; }
        public string PhysicianLastName { get; init; }
        public string PhysicianEmail { get; init; }
    }
}
