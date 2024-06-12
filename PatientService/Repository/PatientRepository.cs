using PatientService.Domain;

namespace PatientService.Repository
{
    public class PatientRepository
    {
        private PatientDBContext dbContext;
        public PatientRepository(PatientDBContext dBContext)
        {
            this.dbContext = dBContext;
        }

        public void Post(Patient patient) { 
            dbContext.Patients.Add(patient);
        }
        public void Put(Patient patient) { }
        public IEnumerable<Patient>? GetAll() { return dbContext.Patients; }
        public Patient? GetById(int id) { return null; }
    }
}
