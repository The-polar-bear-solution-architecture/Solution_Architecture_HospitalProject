using AppointmentService.CommandsAndEvents.Commands;
using AppointmentService.CommandsAndEvents.Events;
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
        private readonly AppointmentCommandHandler commandHandler;
        public AppointmentController(IAppointmentRepository repo, IPhysicianRepository physicianRepo, IPatientRepository patientRepo, AppointmentCommandHandler commandHandler)
        {
            this.repo = repo;
            this.physicianRepo = physicianRepo;
            this.patientRepo = patientRepo;
            this.commandHandler = commandHandler;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAllAppointments()
        {
            Console.WriteLine("Dit is een test");
            return Ok(repo.GetAllAppointments());
        }

        [HttpGet("{Id:Guid}")]
        public ActionResult<Appointment> GetAppointmentById(Guid Id)
        {
            var appointment = repo.GetAppointmentById(Id);
            if(appointment == null) { 
                return NotFound();
            }
            return Ok(appointment); 
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentDTO appointmentDTO) {
            var appointmentToAdd = TurnDTOToAppointment(appointmentDTO);


            try
            {
                var createdAppointment = repo.AddAppointment(appointmentToAdd);
                var appointmentCreated = new AppointmentCreated()
                {
                    AppointmentId = Guid.NewGuid(),
                    ApointmentName = createdAppointment.Name,
                    AppointmentDate = createdAppointment.AppointmentDate,
                    PatientId = createdAppointment.Patient.Id,
                    PatientFirstName = createdAppointment.Patient.FirstName,
                    PatientLastName = createdAppointment.Patient.LastName,
                    PhysicianId = createdAppointment.Physician.Id,
                    PhysicianFirstName = createdAppointment.Physician.FirstName,
                    PhysicianLastName = createdAppointment.Physician.LastName,
                    PhysicianEmail = createdAppointment.Physician.Email,

                };
                await commandHandler.AppointmentCreated(appointmentCreated);
                return Ok(createdAppointment);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{Id:Guid}")]
        public async Task<ActionResult<Appointment>> UpdateAppointment(Guid Id, AppointmentDTO appointmentDTO)
        {
            Appointment appointmentToUpdate = TurnDTOToAppointment(appointmentDTO);
            appointmentToUpdate.Id = Id;
            try
            {
                var updatedAppointment = repo.UpdateAppointment(appointmentToUpdate);
                var appointmentUpdated = new AppointmentUpdated()
                {
                    AppointmentId = updatedAppointment.Id,
                    ApointmentName = updatedAppointment.Name,
                    AppointmentDate = updatedAppointment.AppointmentDate,
                    PatientId = updatedAppointment.Patient.Id,
                    PatientFirstName = updatedAppointment.Patient.FirstName,
                    PatientLastName = updatedAppointment.Patient.LastName,
                    PhysicianId = updatedAppointment.Physician.Id,
                    PhysicianFirstName = updatedAppointment.Physician.FirstName,
                    PhysicianLastName = updatedAppointment.Physician.LastName,
                    PhysicianEmail = updatedAppointment.Physician.Email,

                };

                await commandHandler.AppointmentUpdated(appointmentUpdated);

                return Ok(appointmentToUpdate);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<ActionResult> DeleteAppointment(Guid Id)
        {
            try
            {
                repo.DeleteAppointment(Id);
                var appointmentDeleted = new AppointmentDeleted()
                {
                    AppointmentId = Id
                };

                await commandHandler.AppointmentDeleted(appointmentDeleted);
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
                previousAppointment = repo.GetAppointmentById((Guid) appointmentDTO.PreviousAppointmentId);
            }
            if(appointmentDTO.Id != null)
            {
                Appointment appointment = new Appointment()
                {
                    Id = (Guid)appointmentDTO.Id,
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
