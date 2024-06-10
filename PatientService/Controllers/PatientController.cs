using PatientService.DTO;

namespace PatientService.Controllers
{
    public class PatientController
    {
        public void Post(PatientDTO commandModel) { }
        public void Put(PatientDTO commandModel) { }
        public IEnumerable<PatientDTO>? GetAll() { return null; }
        public PatientDTO? GetById(int id) { return null; }
    }
}
