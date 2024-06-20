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
        private IGeneralPractitionerRepository generalPractitionerRepository;
        public PatientController(IPatientRepository patientRepository, IGeneralPractitionerRepository generalPractitionerRepository)
        {
            this.patientRepository = patientRepository;
            this.generalPractitionerRepository = generalPractitionerRepository;
        }

        [HttpPost]
        public ActionResult<Patient> Post(PatientDTO commandModel) {
            var patient = TurnDTOToPatient(commandModel);
            try
            {
                if (patient == null) { return BadRequest("Patient could not been added"); }
                patientRepository.Post(patient);
                return Ok(patient);
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

        private Patient? TurnDTOToPatient(PatientDTO patientDTO)
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
                    var generalPractitioner = generalPractitionerRepository.GetByEmail(patientDTO.GeneralPractionerEmail);
                    patient.GeneralPractitioner = generalPractitioner;
                    return patient;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }
    }
}
