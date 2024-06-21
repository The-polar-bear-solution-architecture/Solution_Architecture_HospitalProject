using CheckinService.Model;
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
            return new AppointmentReadUpdateCommand()
            {
                AppointmentDate = updateCheckInDTO.AppointmentDate,
                AppointmentName = updateCheckInDTO.ApointmentName,
                AppointmentSerialNr = updateCheckInDTO.AppointmentId,
                PhysicianSerialNr = updateCheckInDTO.PhysicianId,
                PhysicianEmail = updateCheckInDTO.PhysicianLastName,
                PhysicianFirstName = updateCheckInDTO.PhysicianFirstName,
                PhysicianLastName = updateCheckInDTO.PhysicianLastName
            };
        }

        public static AppointmentUpdateEvent MapToUpdatedEvent(this AppointmentUpdateCommand appointmentUpdateCommand, Physician? newPhysician)
        {
            if(newPhysician.PhysicianSerialNr.Equals(appointmentUpdateCommand.PhysicianSerialNr))
            {
                return new AppointmentUpdateEvent(nameof(AppointmentUpdateEvent))
                {
                    AppointmentDate = appointmentUpdateCommand.AppointmentDate,
                    AppointmentName = appointmentUpdateCommand.AppointmentName,
                    AppointmentSerialNr = appointmentUpdateCommand.AppointmentSerialNr,
                    PhysicianSerialNr = appointmentUpdateCommand.PhysicianSerialNr
                };
            }
            else
            {
                // The message type of this class will use its parent class, because the process in the ETL depends upon it.
                return new AppointmentReadUpdateEvent(nameof(AppointmentUpdateEvent))
                {
                    PhysicianEmail = newPhysician.Email,
                    PhysicianFirstName = newPhysician.FirstName,
                    PhysicianLastName = newPhysician.LastName,
                    PhysicianSerialNr = newPhysician.PhysicianSerialNr,
                    AppointmentDate = appointmentUpdateCommand.AppointmentDate,
                    AppointmentName = appointmentUpdateCommand.AppointmentName,
                    AppointmentSerialNr = appointmentUpdateCommand.AppointmentSerialNr
                };
            }
            
        }

        public static AppointmentDeleteCommand MapToDeleteEvent(this Guid AppointmentSerialNr, Guid CheckInSerialNr)
        {
            return new AppointmentDeleteCommand()
            {
                AppointmentId = AppointmentSerialNr,
                CheckInSerialNr = CheckInSerialNr
            };
        }

        public static AppointmentDeleteEvent MapToAppointmentDeleted(this AppointmentDeleteCommand appointmentDeleteEvent) 
        {
            return new AppointmentDeleteEvent(nameof(AppointmentDeleteEvent)) { 
                AppointmentSerialNr = appointmentDeleteEvent.AppointmentId,
                CheckInSerialNr = appointmentDeleteEvent.CheckInSerialNr
            };
        }
    }
}
