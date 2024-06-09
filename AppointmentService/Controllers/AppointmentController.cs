using AppointmentService.DB;
using AppointmentService.Domain;
using AppointmentService.Domain.DTO;
using AppointmentService.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        //Temporary, will be replaced with a service
        private readonly IAppointmentRepository repo;
        private readonly IPhysicianRepository physicianRepo;
        private readonly IPatientRepository patientRepo;
        public AppointmentController(IAppointmentRepository repo, IPhysicianRepository physicianRepo, IPatientRepository patientRepo)
        {
            this.repo = repo;
            this.physicianRepo = physicianRepo;
            this.patientRepo = patientRepo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAllAppointments()
        {
            return Ok(repo.GetAllAppointments());
        }

        [HttpGet("{Id:int}")]
        public ActionResult<Appointment> GetAppointmentById(int Id)
        {
            var appointment = repo.GetAppointmentById(Id);
            if(appointment == null) { 
                return NotFound();
            }
            return Ok(appointment); 
        }

        [HttpPost]
        public ActionResult<Appointment> PostAppointment(AppointmentDTO appointmentDTO) {
            var appointmentToAdd = TurnDTOToAppointment(appointmentDTO);
            try
            {
                repo.AddAppointment(appointmentToAdd);
                return Ok(appointmentToAdd);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{Id:int}")]
        public ActionResult<Appointment> UpdateAppointment(int Id, AppointmentDTO appointmentDTO)
        {
            Appointment appointmentToUpdate = TurnDTOToAppointment(appointmentDTO);
            appointmentToUpdate.Id = Id;
            try
            {
                repo.UpdateAppointment(appointmentToUpdate);
                return Ok(appointmentToUpdate);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public ActionResult DeleteAppointment(int Id)
        {
            try
            {
                repo.DeleteAppointment(Id);
                return Ok("Appointment deleted");
            } catch (Exception e) { 
                return BadRequest(e.Message);
            }
        }

        private Appointment TurnDTOToAppointment(AppointmentDTO appointmentDTO)
        {
            Appointment previousAppointment = null;
            if (appointmentDTO.PreviousAppointmentId != null)
            {
                previousAppointment = repo.GetAppointmentById((int)appointmentDTO.PreviousAppointmentId);
            }
            if(appointmentDTO.Id != null)
            {
                Appointment appointment = new Appointment()
                {
                    Id = (int)appointmentDTO.Id,
                    Name = appointmentDTO.Name,
                    AppointmentDate = appointmentDTO.AppointmentDate,
                    Physician = physicianRepo.GetPhysicianById(appointmentDTO.PhysicianId),
                    Patient = patientRepo.GetPatientById(appointmentDTO.PatientId),
                    PreviousAppointment = previousAppointment

                };
                return appointment;
            } else
            {
                Appointment appointment = new Appointment()
                {
                    Name = appointmentDTO.Name,
                    AppointmentDate = appointmentDTO.AppointmentDate,
                    Physician = physicianRepo.GetPhysicianById(appointmentDTO.PhysicianId),
                    Patient = patientRepo.GetPatientById(appointmentDTO.PatientId),
                    PreviousAppointment = previousAppointment

                };
                return appointment;
            }


        }
        
    }
}
