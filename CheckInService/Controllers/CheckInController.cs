using CheckinService.Model;
using CheckInService.Commands;
using CheckInService.Mapper;
using CheckInService.Models;
using CheckInService.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheckInService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        private readonly CheckInRepository checkInRepository;

        public CheckInController(CheckInRepository checkInRepository) {
            this.checkInRepository = checkInRepository;
        }

        // GET: api/<CheckInController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(checkInRepository.Get());
        }

        // GET api/<CheckInController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var checkIn = checkInRepository.Get(id);
            if(checkIn == null)
            {
                return BadRequest("CheckIn not found");
            }
            return Ok(checkIn);
        }

        // PUT api/<CheckInController>/5
        [HttpPut("{id}/MarkNoShow")]
        public IActionResult PutNoShow(int id)
        {
            CheckIn? checkIn = checkInRepository.Get(id);
            if(checkIn == null)
            {
                return NotFound();
            }

            checkIn.Status = Status.NOSHOW;
            checkInRepository.Put(checkIn);

            return Ok("Marked appointment as noshow");
        }

        [HttpPut("{id}/MarkPresent")]
        public IActionResult PutPresent(int id)
        {
            CheckIn? checkIn = checkInRepository.Get(id);
            if (checkIn == null)
            {
                return NotFound();
            }
            checkIn.Status = Status.PRESENT;
            checkInRepository.Put(checkIn);
            // Event to notification physician.
            Console.WriteLine("Physician will be notified");
            return Ok("Marked check-in ready");
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] CreateCheckInCommand createCheckInCommand)
        {
            CheckIn checkIn = createCheckInCommand.MapToCheckin();

            checkInRepository.Post(checkIn);
            
            return Ok("Marked check-in ready");
        }
    }
}
