using Microsoft.AspNetCore.Mvc;
using PatientService.Domain;
using PatientService.DomainServices;

namespace PatientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository patientRepository;
        public PatientController(IPatientRepository patientRepository)
        {
            this.patientRepository = patientRepository;
        }

/*          [HttpPost]
        public void Post(PatientDTO commandModel)
        {

        }
      [HttpPut("{id:int}")]
        public ActionResult<Patient> Put(int id, PatientDTO commandModel)
        {
            try
            {
                if (id == commandModel.Id)
                {
                    //get GP
                    GeneralPractitioner generalPractitioner = new GeneralPractitioner();
                    var myPatient = MapPatient(commandModel, new Patient(), generalPractitioner);
                    patientRepository.Put(myPatient);
                    return Ok(myPatient);
                } else
                {
                    throw new Exception("Not Found");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }*/
        [HttpGet]
        public ActionResult<IEnumerable<Patient>>? GetAll() { return Ok(patientRepository.GetAll()); }
        [HttpGet("{id:int}")]
        public ActionResult<Patient>? GetById(int id) { return Ok(patientRepository.GetById(id)); }
    }
}
