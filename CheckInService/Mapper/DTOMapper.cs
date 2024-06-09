using CheckInService.Commands;
using CheckInService.Models;

namespace CheckInService.Mapper
{
    public static class DTOMapper
    {
        public static CheckIn MapToCheckin(this CreateCheckInCommand createCheckInCommand)
        {
            return new CheckIn
            {
                Appointment = new Appointment()
                {
                    Name = createCheckInCommand.ApointmentName,
                    AppointmentDate = createCheckInCommand.AppointmentDate,
                    Patient = new Patient()
                    {
                        FirstName = createCheckInCommand.PatientFirstName,
                        LastName = createCheckInCommand.PatientLastName,
                    },
                    Physician = new CheckinService.Model.Physician()
                    {
                        FirstName = createCheckInCommand.PhysicianFirstName,
                        LastName = createCheckInCommand.PatientLastName,
                        Email = createCheckInCommand.PhysicianEmail
                    }
                }
            };
        }
    }
}
