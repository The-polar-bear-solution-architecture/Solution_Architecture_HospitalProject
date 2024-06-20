using PatientService.Domain;
using PatientService.DomainServices;

namespace PatientService.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDBContext patientDBContext;
        public PatientRepository(PatientDBContext patientDBContext)
        {
            this.patientDBContext = patientDBContext;
        }
        public void Post(Patient patient) 
        {
            patientDBContext.Add(patient);
            patientDBContext.SaveChanges();
        }
        public void Put(Patient patient) 
        {
            patientDBContext.Patients.Update(patient);
            patientDBContext.SaveChanges();
        }
        public IEnumerable<Patient>? GetAll() { return patientDBContext.Patients.ToList(); }
        public Patient? GetById(Guid id) { return patientDBContext.Patients.Where(x => x.Id == id).FirstOrDefault(); }
    }
}
