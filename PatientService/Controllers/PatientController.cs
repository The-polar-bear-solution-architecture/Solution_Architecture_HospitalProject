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
        public ActionResult<Patient> Post(PatientDTO commandModel) {
            try
            {
                var x = new Patient();
                x.FirstName = commandModel.FirstName;
                x.LastName = commandModel.LastName;
                x.Email = commandModel.Email;
                x.DateOfBirth = commandModel.DateOfBirth;
                x.BSN = commandModel.BSN;
                patientRepository.Post(x);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public void Put(PatientDTO commandModel) { }


        [HttpGet]
        public IEnumerable<Patient>? GetAll() { return patientRepository.GetAll(); }

        [HttpGet("id")]
        public PatientDTO? GetById(int id) { return null; }

        private Patient TurnDTOToPatient(PatientDTO patientDTO)
        {
            var patient = new Patient();

            if (patientDTO.Id != null)
            {
                patient.Id = Guid.Parse(patientDTO.Id);
                patient.FirstName = patientDTO.FirstName;
                patient.LastName = patientDTO.LastName;
                patient.PhoneNumber = patientDTO.PhoneNumber;
                patient.GeneralPractitioner = null;
            }
            if (patientDTO.GeneralPractionerEmail != null)
            {
                try
                {
                    patientRepository.GetByEmail();
                }
                catch (Exception ex)
                {

                }
            }
            return patient;
        }
    }
}
