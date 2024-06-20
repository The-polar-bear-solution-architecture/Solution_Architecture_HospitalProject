using Microsoft.AspNetCore.Mvc;
using PatientService.Domain;
using PatientService.DomainServices;
using PatientService.DTO;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private IPatientRepository patientRepository;
        public PatientController(IPatientRepository patientRepository)
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

        [HttpPut]
        public void Put(PatientDTO commandModel) { }


        [HttpGet]
        public IEnumerable<Patient>? GetAll() { return patientRepository.GetAll(); }

        [HttpGet("id")]
        public PatientDTO? GetById(int id) { return null; }
    }
}
