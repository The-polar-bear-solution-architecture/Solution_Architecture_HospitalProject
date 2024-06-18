using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.Models;
using CheckInService.Models.DTO;

namespace CheckInService.Mapper
{
    public static class AppointmentMapper
    { 

        public static AppointmentUpdateCommand MapToAppointmentUpdateCommand(this UpdateCheckInDTO updateCheckInDTO)
        {
            return new AppointmentUpdateCommand()
            {
                AppointmentDate = updateCheckInDTO.AppointmentDate,
                AppointmentName = updateCheckInDTO.ApointmentName,
                AppointmentSerialNr = updateCheckInDTO.AppointmentGuid,
                PhysicianSerialNr = updateCheckInDTO.PhysicianGuid,
            };
        }

        public static AppointmentUpdateEvent MapToUpdatedEvent(this AppointmentUpdateCommand appointmentUpdateCommand)
        {
            return new AppointmentUpdateEvent(nameof(AppointmentUpdateEvent))
            {
                AppointmentDate = appointmentUpdateCommand.AppointmentDate,
                AppointmentSerialNr = appointmentUpdateCommand.AppointmentSerialNr,
                PhysicianSerialNr = appointmentUpdateCommand.PhysicianSerialNr
            };
        }

        public static AppointmentDeleteCommand MapToDeleteEvent(this Guid AppointmentSerialNr, Guid CheckInSerialNr)
        {
            return new AppointmentDeleteCommand()
            {
                AppointmentSerialNr = AppointmentSerialNr,
                CheckInSerialNr = CheckInSerialNr
            };
        }

        public static AppointmentDeleteEvent MapToAppointmentDeleted(this AppointmentDeleteCommand appointmentDeleteEvent) 
        {
            return new AppointmentDeleteEvent(nameof(AppointmentDeleteEvent)) { 
                AppointmentSerialNr = appointmentDeleteEvent.AppointmentSerialNr,
                CheckInSerialNr = appointmentDeleteEvent.CheckInSerialNr
            };
        }
    }
}
