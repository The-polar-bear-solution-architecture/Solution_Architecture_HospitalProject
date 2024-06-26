using PatientService.Domain;
using PatientService.DomainServices;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Messages.Mapper;
using Polly;

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
        public Patient Put(Patient patient)
        {
            patientDBContext.Patients.Update(patient);
            patientDBContext.SaveChanges();
            return patient;
        }

        public void Delete(Patient patient)
        {
            patientDBContext.Patients.Remove(patient);
            patientDBContext.SaveChanges();
        }
        public IEnumerable<Patient>? GetAll() { return patientDBContext.Patients.Include(x => x.GeneralPractitioner).ToList(); }
        public Patient? GetById(Guid id) { return patientDBContext.Patients.Include(x => x.GeneralPractitioner).Where(x => x.Id == id).FirstOrDefault(); }

        public Patient GetByDetails(string firstName, string lastName, string phoneNumber)
        {
            return patientDBContext.Patients.FirstOrDefault(p => p.FirstName == firstName && p.LastName == lastName && p.PhoneNumber == phoneNumber);
        }
    }
}
