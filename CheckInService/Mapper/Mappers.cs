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
            var appointment_guid = createCheckInCommand.AppointmentGuid;
            var physician_guid = createCheckInCommand.PhysicianGuid;
            var patient_guid = createCheckInCommand.PatientGuid;

            return new CheckIn
            {
                Appointment = new Appointment()
                {
                    Name = createCheckInCommand.ApointmentName,
                    AppointmentDate = createCheckInCommand.AppointmentDate,
                    AppointmentSerialNr = appointment_guid,
                    Patient = new Patient()
                    {
                        PatientSerialNr = patient_guid,
                        FirstName = createCheckInCommand.PatientFirstName,
                        LastName = createCheckInCommand.PatientLastName,
                    },
                    Physician = new Physician()
                    {
                        PhysicianSerialNr = physician_guid,
                        FirstName = createCheckInCommand.PhysicianFirstName,
                        LastName = createCheckInCommand.PatientLastName,
                        Email = createCheckInCommand.PhysicianEmail
                    }
                }
            };
        }

        public static CheckIn MapToRegister(this RegisterCheckin createCheckInCommand)
        {
            var appointment_guid = createCheckInCommand.AppointmentGuid;
            var physician_guid = createCheckInCommand.PhysicianGuid;
            var patient_guid = createCheckInCommand.PatientGuid;

            return new CheckIn
            {
                SerialNr = createCheckInCommand.CheckinSerialNr,
                Appointment = new Appointment()
                {
                    AppointmentSerialNr = appointment_guid,
                    Name = createCheckInCommand.ApointmentName,
                    AppointmentDate = createCheckInCommand.AppointmentDate,
                    Patient = new Patient()
                    {
                        PatientSerialNr = patient_guid,
                        FirstName = createCheckInCommand.PatientFirstName,
                        LastName = createCheckInCommand.PatientLastName,
                    },
                    Physician = new Physician()
                    {
                        PhysicianSerialNr = physician_guid,
                        FirstName = createCheckInCommand.PhysicianFirstName,
                        LastName = createCheckInCommand.PhysicianLastName,
                        Email = createCheckInCommand.PhysicianEmail
                    }
                },
                Status = Status.AWAIT
            };
        }

        public static RegisterCheckin MapToRegister(this CreateCheckInCommandDTO createCheckInCommand)
        {
            var apppointment_guid = createCheckInCommand.AppointmentGuid;
            var physician_guid = createCheckInCommand.PhysicianGuid;
            var patient_guid = createCheckInCommand.PatientGuid;

            return new RegisterCheckin(Guid.NewGuid(), nameof(RegisterCheckin))
            {
                PatientId = createCheckInCommand.PatientId,
                PhysicianId = createCheckInCommand.PhysicianId,
                AppointmentId = createCheckInCommand.AppointmentId,
                AppointmentGuid = apppointment_guid,
                ApointmentName = createCheckInCommand.ApointmentName,
                AppointmentDate = createCheckInCommand.AppointmentDate,
                PatientGuid = physician_guid,
                PatientFirstName = createCheckInCommand.PatientFirstName,
                PatientLastName = createCheckInCommand.PatientLastName,
                PhysicianGuid = physician_guid,
                PhysicianEmail = createCheckInCommand.PhysicianEmail,
                PhysicianFirstName = createCheckInCommand.PhysicianFirstName,
                PhysicianLastName = createCheckInCommand.PhysicianLastName,
                Status = Status.AWAIT
            };
        }

        public static CheckInRegistrationEvent MapCheckinRegistered(this RegisterCheckin createCheckInCommand, int checkInId, string serialNr, int apointmentId)
        {
            var apppointment_guid = createCheckInCommand.AppointmentGuid;
            var physician_guid = createCheckInCommand.PhysicianGuid;
            var patient_guid = createCheckInCommand.PatientGuid;

            return new CheckInRegistrationEvent(Guid.NewGuid(), nameof(CheckInRegistrationEvent))
            {
                CheckInId = checkInId,
                CheckInSerialNr = serialNr,
                AppointmentId = apointmentId,
                AppointmentGuid = apppointment_guid,
                ApointmentName = createCheckInCommand.ApointmentName,
                AppointmentDate = createCheckInCommand.AppointmentDate,
                PatientId = createCheckInCommand.PatientId,
                PatientGuid = patient_guid,
                PatientFirstName = createCheckInCommand.PatientFirstName,
                PatientLastName = createCheckInCommand.PatientLastName,
                PhysicianId = createCheckInCommand.PhysicianId,
                PhysicianGuid = physician_guid,
                PhysicianEmail = createCheckInCommand.PhysicianEmail,
                PhysicianFirstName = createCheckInCommand.PhysicianFirstName,
                PhysicianLastName = createCheckInCommand.PatientLastName,
                Status = Status.AWAIT
            };
        }

        public static CheckInPresentEvent MapToPatientIsPresent(this CheckIn checkIn)
        {
            return new CheckInPresentEvent(Guid.NewGuid(), nameof(CheckInPresentEvent))
            {
                CheckInId = checkIn.Id,
                CheckInSerialNr = checkIn.SerialNr,
                Status = checkIn.Status
            };
        }

        public static CheckInNoShowEvent MapPatientNoShow(this CheckIn checkIn)
        {
            return new CheckInNoShowEvent(Guid.NewGuid(), nameof(CheckInNoShowEvent))
            {
                CheckInId = checkIn.Id,
                CheckInSerialNr = checkIn.SerialNr,
                Status = checkIn.Status
            };
        }
    }
}
