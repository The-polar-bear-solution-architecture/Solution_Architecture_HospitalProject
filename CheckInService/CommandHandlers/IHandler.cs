using CheckInService.CommandsAndEvents.Commands;
using CheckInService.Models;

namespace CheckInService.CommandHandlers
{
    public interface IHandler
    {
        Task<CheckIn?> ChangeToPresent(PresentCheckin command);
        Task<CheckIn?> ChangeToNoShow(NoShowCheckIn command);
        Task<CheckIn> RegisterCheckin(RegisterCheckin command);
    }
}
