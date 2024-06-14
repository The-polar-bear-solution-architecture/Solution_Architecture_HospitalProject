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
                SerialNr = createCheckInCommand.CheckinSerialNr,
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
                PatientId = createCheckInCommand.PatientId,
                PhysicianId = createCheckInCommand.PhysicianId,
                AppointmentId = createCheckInCommand.AppointmentId,
                ApointmentName = createCheckInCommand.ApointmentName,
                AppointmentDate = createCheckInCommand.AppointmentDate,
                PatientFirstName = createCheckInCommand.PatientFirstName,
                PatientLastName = createCheckInCommand.PatientLastName,
                PhysicianEmail = createCheckInCommand.PhysicianEmail,
                PhysicianFirstName = createCheckInCommand.PhysicianFirstName,
                PhysicianLastName = createCheckInCommand.PhysicianLastName,
                Status = Status.AWAIT
            };
        }

        public static PatientCheckinRegistered MapCheckinRegistered(this RegisterCheckin createCheckInCommand, int checkInId, string serialNr)
        {
            return new PatientCheckinRegistered(Guid.NewGuid(), nameof(PatientCheckinRegistered))
            {
                CheckInId = checkInId,
                CheckInSerialNr = serialNr,
                ApointmentName = createCheckInCommand.ApointmentName,
                AppointmentDate = createCheckInCommand.AppointmentDate,
                PatientId = createCheckInCommand.PatientId,
                PatientFirstName = createCheckInCommand.PatientFirstName,
                PatientLastName = createCheckInCommand.PatientLastName,
                PhysicianId = createCheckInCommand.PhysicianId,
                PhysicianEmail = createCheckInCommand.PhysicianEmail,
                PhysicianFirstName = createCheckInCommand.PhysicianFirstName,
                PhysicianLastName = createCheckInCommand.PatientLastName,
                Status = Status.AWAIT
            };
        }

        public static PatientHasCheckedIn MapToPatientIsPresent(this CheckIn checkIn)
        {
            Console.WriteLine("Change to event");
            return new PatientHasCheckedIn(Guid.NewGuid(), nameof(PatientHasCheckedIn))
            {
                CheckInId = checkIn.Id,
                Status = checkIn.Status,
                PatientFirstName = checkIn.Appointment.Patient.FirstName,
                PatientLastName = checkIn.Appointment.Patient.LastName,
                PhysicianFirstName = checkIn.Appointment.Physician.FirstName,
                PhysicianLastName = checkIn.Appointment.Physician.LastName,
                PhysicianEmail = checkIn.Appointment.Physician.Email,
            };
        } 
    }
}
