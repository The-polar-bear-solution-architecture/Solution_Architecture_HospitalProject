using CheckInService.Models;
using EventStore.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Messages.Mapper;
using System.Threading;
using System.Text.Json;
using CheckInService.CommandsAndEvents.Commands;
using RabbitMQ.Messages.Messages;
using Newtonsoft.Json;
using static EventStore.Client.StreamMessage;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CheckInService.DBContexts;
using Microsoft.EntityFrameworkCore;
using CheckInService.CommandHandlers;
using Event = RabbitMQ.Messages.Messages.Event;
using CheckInService.CommandsAndEvents.Events.CheckIn;
using CheckInService.CommandsAndEvents.Events.Appointment;
using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using RabbitMQ.Messages.Interfaces;
using CheckInService.Configurations;

namespace CheckInService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplayController : ControllerBase
    {
        private readonly CheckInContextDB checkInContext;
        private readonly IPublisher InternalPublisher;
        private readonly string RouterKey;

        public ReplayController(
            CheckInContextDB checkInContext,
            IRabbitFactory rabbitFactory)
        {
            this.checkInContext = checkInContext;
            this.InternalPublisher = rabbitFactory.CreateInternalPublisher();
            RouterKey = "ETL_Checkin";
        }

        [HttpDelete(Name = "ClearData")]
        public async Task<IActionResult> ClearCheckInDatabase()
        {
            // For now only the checkin table will be cleared.
            // Deletes all elements from checkin 
            var checkIns = checkInContext.checkIns.ToList();
            var tempAppointments = checkInContext.Appointments.ToList();
            var physicians = checkInContext.Physicians.ToList();
            var patients = checkInContext.Patients.ToList();
            checkInContext.checkIns.RemoveRange(checkIns);
            checkInContext.Appointments.RemoveRange(tempAppointments);
            checkInContext.Physicians.RemoveRange(physicians);
            checkInContext.Patients.RemoveRange(patients);

            checkInContext.SaveChanges();
            // Later on, all the other tables will be cleared too.

            await InternalPublisher.SendMessage("Clear" , "", RouterKey);
            return Ok("All checkIns have been deleted.");
        }

        [HttpPut(Name = "Synchronize")]
        public async Task<IActionResult> ReplayAll()
        {
            // Tells ETL Worker to synchronize the data between WriteDB/Event source with current read database.
            await InternalPublisher.SendMessage("Replay", "", RouterKey);
            var responseData =
            new {
                Text = "Data will now be synchronised with the read database.",
                Date = DateTime.Now,
            };
            return Ok(responseData);
        }
    }
}
