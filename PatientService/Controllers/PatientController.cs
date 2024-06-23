using Microsoft.AspNetCore.Mvc;
using PatientService.Domain;
using PatientService.DomainServices;
using PatientService.DTO;
using PatientService.Repository;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private IPatientRepository patientRepository;
        private EventStoreRepository eventStoreRepository;
        private IGeneralPractitionerRepository generalPractitionerRepository;
        private PatientWorker patientWorker;
        public PatientController(IPatientRepository patientRepository, IGeneralPractitionerRepository generalPractitionerRepository, EventStoreRepository eventStoreRepository, PatientWorker patientWorker)
        {
            this.patientRepository = patientRepository;
            this.generalPractitionerRepository = generalPractitionerRepository;
            this.eventStoreRepository = eventStoreRepository;
            this.patientWorker = patientWorker;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> Post(PatientDTO commandModel)
        {
            var patient = TurnDTOToPatient(commandModel);
            try
            {
                if (patient == null) { return BadRequest("Patient could not been added"); }
                patientRepository.Post(patient);
                await eventStoreRepository.HandlePatientCreatedEvent(patient);
                await patientWorker.SendMessageAsync("POST", patient);
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<GeneralPractitioner>> UpdateGeneralPractitioner(string Id, PatientDTO commandModel)
        {
            Patient? patient = TurnDTOToPatient(commandModel);
            if (patient == null || patient.Id != Guid.Parse(Id)) { return BadRequest("Not Found"); }
            try
            {
                patientRepository.Put(patient);
                await eventStoreRepository.HandlePatientUpdatedEvent(patient);
                await patientWorker.SendMessageAsync("PUT", patient);
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteById(string Id)
        {
            try
            {
                var patient = patientRepository.GetById(Guid.Parse(Id));
                if(patient == null) { return NotFound(); }
                patientRepository.Delete(patient);
                await eventStoreRepository.HandlePatientDeletedEvent(patient);
                await patientWorker.SendMessageAsync("DELETE", patient.Id);
                return Ok(patient.Id);
            } catch (Exception ex)
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
                    if (generalPractitioner == null) { return null; }
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
