using Microsoft.AspNetCore.Mvc;
using PatientService.Domain;
using PatientService.DomainServices;
using PatientService.DTO;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralPractitionerController : ControllerBase
    {
        private IGeneralPractitionerRepository generalPractitionerRepository;
        public GeneralPractitionerController(IGeneralPractitionerRepository generalPractitionerRepository)
        {
            this.generalPractitionerRepository = generalPractitionerRepository;
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
        public ActionResult<GeneralPractitioner> Post(GeneralPractitionerDTO commandModel)
        {
            var gp = TurnDTOToGeberalPractitioner(commandModel);
            try
            {
                if (gp == null) { return BadRequest("GeneralPractitioner could not been added"); }
                generalPractitionerRepository.Post(gp);
                return Ok(gp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{Id}")]
        public ActionResult<GeneralPractitioner> UpdateGeneralPractitioner(string Id, GeneralPractitionerDTO commandModel)
        {
            GeneralPractitioner? gp = TurnDTOToGeberalPractitioner(commandModel);
            if (gp == null || gp.Id != Guid.Parse(Id)) { return BadRequest("Not Found"); }
            try
            {
                generalPractitionerRepository.Put(gp);
                return Ok("General practitioner updated!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
