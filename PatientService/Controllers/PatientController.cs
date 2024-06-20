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

        [HttpPut("{Id}")]
        public ActionResult<GeneralPractitioner> UpdateGeneralPractitioner(string Id, PatientDTO commandModel)
        {
            Patient? patient = TurnDTOToPatient(commandModel);
            if (patient == null || patient.Id != Guid.Parse(Id)) { return BadRequest("Not Found"); }
            try
            {
                patientRepository.Put(patient);
                return Ok("Patient is updated!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetAllPatients() { return Ok(patientRepository.GetAll()); }

        [HttpGet("{Id}")]
        public ActionResult<Patient> GetById(string Id) 
        {
            try
            {
                var patient = patientRepository.GetById(Guid.Parse(Id));
                if (patient == null) { return NotFound(); }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private Patient? TurnDTOToPatient(PatientDTO patientDTO)
        {
            var patient = new Patient();

            if (patientDTO.Id != null)
            {
                patient.Id = Guid.Parse(patientDTO.Id);
            }
            if (patientDTO.GeneralPractionerEmail != null)
            {
                try
                {
                    var generalPractitioner = generalPractitionerRepository.GetByEmail(patientDTO.GeneralPractionerEmail);
                    if (generalPractitioner == null) { return null;  }
                    patient.FirstName = patientDTO.FirstName;
                    patient.LastName = patientDTO.LastName;
                    patient.PhoneNumber = patientDTO.PhoneNumber;
                    patient.Address = patientDTO.Address;
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
