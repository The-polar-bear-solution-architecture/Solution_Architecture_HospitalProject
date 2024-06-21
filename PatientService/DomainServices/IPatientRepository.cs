using PatientService.Domain;

namespace PatientService.DomainServices
{
    public interface IPatientRepository
    {
        public void Post(Patient patient);
        public void Put(Patient patient);
        public IEnumerable<Patient>? GetAll();
        public Patient? GetById(Guid id);
        public void Delete(Patient patient);
    }
}
