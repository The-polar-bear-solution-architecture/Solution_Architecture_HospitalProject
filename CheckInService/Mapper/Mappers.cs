using CheckinService.Model;
using CheckInService.CommandsAndEvents.Commands;
using CheckInService.CommandsAndEvents.Events;
using CheckInService.Models;
using CheckInService.Models.DTO;

namespace CheckInService.Mapper
{
    public static class Mappers
    {
        public static CheckIn MapToCheckin(this CreateCheckInCommandDTO createCheckInCommand)
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
                    Physician = new Physician()
                    {
                        FirstName = createCheckInCommand.PhysicianFirstName,
                        LastName = createCheckInCommand.PatientLastName,
                        Email = createCheckInCommand.PhysicianEmail
                    }
                }
            };
        }

        public static CheckIn MapToRegister(this RegisterCheckin createCheckInCommand)
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
                    Physician = new Physician()
                    {
                        FirstName = createCheckInCommand.PhysicianFirstName,
                        LastName = createCheckInCommand.PatientLastName,
                        Email = createCheckInCommand.PhysicianEmail
                    }
                },
                Status = Status.AWAIT
            };
        }

        public static RegisterCheckin MapToRegister(this CreateCheckInCommandDTO createCheckInCommand)
        {
            return new RegisterCheckin(Guid.NewGuid(), nameof(RegisterCheckin))
            {
                ApointmentName = createCheckInCommand.ApointmentName,
                AppointmentDate = createCheckInCommand.AppointmentDate,
                PatientFirstName = createCheckInCommand.PatientFirstName,
                PatientLastName = createCheckInCommand.PatientLastName,
                PhysicianEmail = createCheckInCommand.PhysicianEmail,
                PhysicianFirstName = createCheckInCommand.PhysicianFirstName,
                PhysicianLastName = createCheckInCommand.PatientLastName,
                Status = Status.AWAIT
            };
        }

        public static PatientCheckinRegistered MapCheckinRegistered(this RegisterCheckin createCheckInCommand)
        {
            return new PatientCheckinRegistered(Guid.NewGuid(), nameof(PatientCheckinRegistered))
            {
                ApointmentName = createCheckInCommand.ApointmentName,
                AppointmentDate = createCheckInCommand.AppointmentDate,
                PatientFirstName = createCheckInCommand.PatientFirstName,
                PatientLastName = createCheckInCommand.PatientLastName,
                PhysicianEmail = createCheckInCommand.PhysicianEmail,
                PhysicianFirstName = createCheckInCommand.PhysicianFirstName,
                PhysicianLastName = createCheckInCommand.PatientLastName,
                Status = Status.AWAIT
            };
        }

        public static PatientHasCheckedIn MapToPatientIsPresent(this CheckIn checkIn)
        {
            return new PatientHasCheckedIn(nameof(PatientHasCheckedIn))
            {
                PatientFirstName = checkIn.Appointment.Patient.FirstName,
                PatientLastName = checkIn.Appointment.Patient.LastName,
                PhysicianFirstName = checkIn.Appointment.Physician.FirstName,
                PhysicianLastName = checkIn.Appointment.Physician.LastName,
                PhysicianEmail = checkIn.Appointment.Physician.Email,
            };
        } 
    }
}
