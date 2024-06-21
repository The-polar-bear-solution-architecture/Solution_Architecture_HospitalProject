using Microsoft.AspNetCore.Mvc;
using PatientService.Domain;
using PatientService.DomainServices;
using PatientService.DTO;
using PatientService.Repository;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralPractitionerController : ControllerBase
    {
        private IGeneralPractitionerRepository generalPractitionerRepository;
        private EventStoreRepository eventStoreRepository;
        public GeneralPractitionerController(IGeneralPractitionerRepository generalPractitionerRepository, EventStoreRepository eventStoreRepository)
        {
            this.generalPractitionerRepository = generalPractitionerRepository;
            this.eventStoreRepository = eventStoreRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<GeneralPractitioner>> GetAllGeneralPractitioners()
        {
            return Ok(generalPractitionerRepository.GetAll());
        }

        [HttpGet("{Id}")]
        public ActionResult<GeneralPractitioner> GetGeneralPractitioneById(string Id)
        {
            try
            {
                var generalPractitioner = generalPractitionerRepository.GetById(Guid.Parse(Id));
                if (generalPractitioner == null) { return NotFound(); }
                return Ok(generalPractitioner);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<GeneralPractitioner>> Post(GeneralPractitionerDTO commandModel)
        {
            var gp = TurnDTOToGeberalPractitioner(commandModel);
            try
            {
                if (gp == null) { return BadRequest("GeneralPractitioner could not been added"); }
                generalPractitionerRepository.Post(gp);
                await eventStoreRepository.HandleGPCreatedEvent(gp);
                return Ok(gp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult<GeneralPractitioner>> UpdateGeneralPractitioner(string Id, GeneralPractitionerDTO commandModel)
        {
            GeneralPractitioner? gp = TurnDTOToGeberalPractitioner(commandModel);
            if (gp == null || gp.Id != Guid.Parse(Id)) { return BadRequest("Not Found"); }
            try
            {
                generalPractitionerRepository.Put(gp);
                await eventStoreRepository.HandleGPUpdatedEvent(gp);
                return Ok("General practitioner updated!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteById(string Id)
        {
            try
            {
                var gp = generalPractitionerRepository.GetById(Guid.Parse(Id));
                if (gp == null) { return NotFound(); }
                generalPractitionerRepository.Delete(gp);
                await eventStoreRepository.HandleGPDeletedEvent(gp);
                return Ok(gp.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private GeneralPractitioner? TurnDTOToGeberalPractitioner(GeneralPractitionerDTO generalPractitionerDTO)
        {
            var gp = new GeneralPractitioner();
            if (generalPractitionerDTO.Id != null)
            {
                gp.Id = Guid.Parse(generalPractitionerDTO.Id);
            }
            gp.FirstName = generalPractitionerDTO.FirstName;
            gp.LastName = generalPractitionerDTO.LastName;
            gp.Email = generalPractitionerDTO.Email;
            gp.PhoneNumber = generalPractitionerDTO.PhoneNumber;
            gp.Address = generalPractitionerDTO.Address;
            return gp;
        }
    }
}
