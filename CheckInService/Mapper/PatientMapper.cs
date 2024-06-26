using CheckInService.CommandsAndEvents.Commands.Patient;
using CheckInService.CommandsAndEvents.Events.Patient;
using CheckInService.Models;

namespace CheckInService.Mapper
{
    public static class PatientMapper
    {

        public static Patient MapCreateCommandToPatient(this PatientCreate patientCreate)
        {
            return new Patient()
            {
                PatientSerialNr = patientCreate.Id,
                FirstName = patientCreate.FirstName,
                LastName = patientCreate.LastName,
            };
        }

        public static PatientCreatedEvent MapPatientCreatedEvent(this PatientCreate patientCreate)
        {
            return new PatientCreatedEvent(nameof(PatientCreatedEvent))
            {
                Id = patientCreate.Id,
                FirstName = patientCreate.FirstName,
                LastName = patientCreate.LastName,
            };
        }

        public static PatientChangeEvent MapPatientUpdateEvent(this PatientUpdate patientUpdate)
        {
            return new PatientChangeEvent(nameof(PatientChangeEvent))
            {
                Id = patientUpdate.Id,
                FirstName = patientUpdate.FirstName,
                LastName = patientUpdate.LastName,
            };
        }

        public static Patient MapUpdateCommandToPatientUpdate(this PatientUpdate patientUpdate)
        {
            return new Patient()
            {
                PatientSerialNr = patientUpdate.Id,
                FirstName = patientUpdate.FirstName,
                LastName = patientUpdate.LastName,
            };
        }
    }
}
