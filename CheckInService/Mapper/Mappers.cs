﻿using CheckinService.Model;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.Models;
using CheckInService.Models.DTO;

namespace CheckInService.Mapper
{
    public static class Mappers
    {
        public static CheckIn MapToCheckin(this CreateCheckInCommandDTO createCheckInCommand)
        {
            var appointment_guid = createCheckInCommand.AppointmentId;
            var physician_guid = createCheckInCommand.PhysicianId;
            var patient_guid = createCheckInCommand.PatientId;

            return new CheckIn
            {
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
                        LastName = createCheckInCommand.PatientLastName,
                        Email = createCheckInCommand.PhysicianEmail
                    }
                }
            };
        }

        public static CheckInReadModel MapToReadModel(this CheckIn checkIn)
        {
            return new CheckInReadModel
            {
                CheckInSerialNr = checkIn.SerialNr,
                Status = checkIn.Status,
                AppointmentGuid = checkIn.Appointment.AppointmentSerialNr,
                ApointmentName = checkIn.Appointment.Name,
                AppointmentDate = checkIn.Appointment.AppointmentDate,
                PatientGuid = checkIn.Appointment.Patient.PatientSerialNr,
                PatientFirstName = checkIn.Appointment.Patient.FirstName,
                PatientLastName = checkIn.Appointment.Patient.LastName,
                PhysicianGuid = checkIn.Appointment.Physician.PhysicianSerialNr,
                PhysicianEmail = checkIn.Appointment.Physician.Email,
                PhysicianFirstName = checkIn.Appointment.Physician.FirstName,
                PhysicianLastName = checkIn.Appointment.Physician.LastName
            };
        }

        public static CheckIn MapToRegister(this RegisterCheckin createCheckInCommand)
        {
            var appointment_guid = createCheckInCommand.AppointmentGuid;
            var physician_guid = createCheckInCommand.PhysicianGuid;
            var patient_guid = createCheckInCommand.PatientGuid;

            return new CheckIn
            {
                Status = createCheckInCommand.Status,
                SerialNr = createCheckInCommand.CheckInSerialNr,
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
            };
        }

        public static RegisterCheckin MapToRegister(this CreateCheckInCommandDTO createCheckInCommand)
        {
            var apppointment_guid = createCheckInCommand.AppointmentId;
            var physician_guid = createCheckInCommand.PhysicianId;
            var patient_guid = createCheckInCommand.PatientId;

            return new RegisterCheckin(Guid.NewGuid(), nameof(RegisterCheckin))
            {
                AppointmentGuid = apppointment_guid,
                ApointmentName = createCheckInCommand.ApointmentName,
                AppointmentDate = createCheckInCommand.AppointmentDate,
                PatientGuid = patient_guid,
                PatientFirstName = createCheckInCommand.PatientFirstName,
                PatientLastName = createCheckInCommand.PatientLastName,
                PhysicianGuid = physician_guid,
                PhysicianEmail = createCheckInCommand.PhysicianEmail,
                PhysicianFirstName = createCheckInCommand.PhysicianFirstName,
                PhysicianLastName = createCheckInCommand.PhysicianLastName,
                Status = Status.AWAIT
            };
        }

        public static CheckInRegistrationEvent MapCheckinRegistered(this RegisterCheckin createCheckInCommand, int checkInId, Guid serialNr, int apointmentId)
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
                CheckInSerialNr = checkIn.SerialNr,
                AppointmentSerialNr = checkIn.Appointment.AppointmentSerialNr,
                Status = checkIn.Status
            };
        }

        public static CheckInNoShowEvent MapPatientNoShow(this CheckIn checkIn)
        {
            return new CheckInNoShowEvent(Guid.NewGuid(), nameof(CheckInNoShowEvent))
            {
                CheckInSerialNr = checkIn.SerialNr,
                AppointmentSerialNr = checkIn.Appointment.AppointmentSerialNr,
                Status = checkIn.Status
            };
        }
    }
}
