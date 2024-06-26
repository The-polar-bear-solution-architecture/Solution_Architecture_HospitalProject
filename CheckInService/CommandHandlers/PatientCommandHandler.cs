using CheckInService.CommandsAndEvents.Commands.Patient;
using CheckInService.CommandsAndEvents.Events.Patient;
using CheckInService.DBContexts;
using CheckInService.Mapper;
using CheckInService.Repositories;

namespace CheckInService.CommandHandlers
{
    public class PatientCommandHandler
    {

        private readonly PatientRepo patientRepo;
        private readonly CheckInContextDB checkInContext;

        public PatientCommandHandler(PatientRepo patientRepo, CheckInContextDB checkInContext)
        {
            this.patientRepo = patientRepo;
            this.checkInContext = checkInContext;
        }

        public PatientCreatedEvent? RegisterPatient(PatientCreate patientCreate)
        {
            try
            {
                patientRepo.Post(patientCreate.MapCreateCommandToPatient());

                return patientCreate.MapPatientCreatedEvent();
            }
            catch
            {
                return null;
            }
        }

        public PatientChangeEvent? ChangePatient(PatientUpdate patientUpdate)
        {
            var patient = patientRepo.Get(patientUpdate.Id);
            if (patient == null)
            {
                return null;
            }

            patient.FirstName = patientUpdate.FirstName;
            patient.LastName = patientUpdate.LastName;

            patientRepo.Put(patient);

            return patientUpdate.MapPatientUpdateEvent();
        }

        public void ClearAll()
        {
            var list = checkInContext.Patients.ToArray();
            checkInContext.Patients.RemoveRange(list);
            checkInContext.SaveChanges(true);
        }
    }
}
