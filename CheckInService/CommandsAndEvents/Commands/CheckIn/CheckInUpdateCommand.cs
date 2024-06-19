using CheckinService.Model;

namespace CheckInService.CommandsAndEvents.Commands.CheckIn
{
    public class CheckInUpdateCommand
    {
        public Guid CheckInSerialNr { get; init; }
        public Status Status { get; init; }
    }
}
