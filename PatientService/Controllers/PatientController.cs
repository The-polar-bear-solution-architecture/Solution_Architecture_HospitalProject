using Microsoft.AspNetCore.Mvc;
using PatientService.Domain;
using PatientService.DTO;
using PatientService.Repository;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController
    {
        private PatientRepository patientRepository;
        public PatientController(PatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        [HttpPost]
        public void Post(PatientDTO commandModel) {
            var x = new Patient();
            x.FirstName = commandModel.FirstName;
            x.LastName = commandModel.LastName;
            x.Email = commandModel.Email;
            x.DateOfBirth = commandModel.DateOfBirth;
            x.BSN = commandModel.BSN;
            patientRepository.Post(x);
        }
        public void Put(PatientDTO commandModel) { }
        [HttpGet]
        public IEnumerable<Patient>? GetAll() { return patientRepository.GetAll(); }
        public PatientDTO? GetById(int id) { return null; }
    }
}
