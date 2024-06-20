using PatientService.Domain;
using PatientService.DomainServices;

namespace PatientService.Repository
{
    public class GeneralPractitionerRepository : IGeneralPractitionerRepository
    {
        private readonly PatientDBContext patientDBContext;
        public GeneralPractitionerRepository(PatientDBContext patientDBContext)
        {
            this.patientDBContext = patientDBContext;
        }
        public void Post(GeneralPractitioner generalPractitioner)
        {
            patientDBContext.Add(generalPractitioner);
            patientDBContext.SaveChanges(true);
        }

        public void Put(GeneralPractitioner generalPractitioner)
        {
            patientDBContext.GeneralPractitioners.Update(generalPractitioner);
            patientDBContext.SaveChanges();
        }

        IEnumerable<GeneralPractitioner>? IGeneralPractitionerRepository.GetAll()
        {
            return patientDBContext.GeneralPractitioners.ToList();
        }

        GeneralPractitioner? IGeneralPractitionerRepository.GetById(Guid id)
        {
            return patientDBContext.GeneralPractitioners.Where(x => x.Id == id).FirstOrDefault();
        }

        public GeneralPractitioner? GetByEmail(string generalPractitionerEmail)
        {
            return patientDBContext.GeneralPractitioners.Where(x => x.Email == generalPractitionerEmail).FirstOrDefault();
        }
    }
}
