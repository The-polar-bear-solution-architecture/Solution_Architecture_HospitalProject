using PatientService.Domain;

namespace PatientService.DomainServices
{
    public interface IGeneralPractitionerRepository
    {
        public void Post(GeneralPractitioner generalPractitioner);
        public void Put(GeneralPractitioner generalPractitioner);
        public IEnumerable<GeneralPractitioner>? GetAll();
        public GeneralPractitioner? GetById(Guid id);
        public GeneralPractitioner? GetByEmail(string generalPractitionerEmail);
    }
}
