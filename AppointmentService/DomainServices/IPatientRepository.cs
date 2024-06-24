using AppointmentService.Domain;

namespace AppointmentService.DomainServices
{
    public interface IPatientRepository
    {
        public Patient GetPatientById(Guid Id);
        public Patient AddPatient (Patient Patient);

        public Patient UpdatePatient (Patient Patient);

        public Task<Patient> DeletePatient(Guid Id);
    }
}
