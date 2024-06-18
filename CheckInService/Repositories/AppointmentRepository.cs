using CheckinService.Model;
using CheckInService.DBContexts;
using CheckInService.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckInService.Repositories
{
    public class AppointmentRepository: IRepo<Appointment>
    {
        private readonly CheckInContextDB checkInContextDB;

        public AppointmentRepository(CheckInContextDB checkInContextDB)
        {
            this.checkInContextDB = checkInContextDB;
        }

        public void Delete(Guid id)
        {
            try
            {
                checkInContextDB.Appointments.Where(e => e.AppointmentSerialNr == id).ExecuteDelete();
            }
            catch
            {
                return;
            }
        }

        public Appointment? Get(int id)
        {
            try
            {
                return checkInContextDB.Appointments.Where(p => p.Id == id).First();
            }
            catch
            {
                return null;
            }
        }

        public Appointment? Get(Guid guid)
        {
            try
            {
                return checkInContextDB.Appointments.Where(p => p.AppointmentSerialNr.Equals(guid)).First();
            }
            catch
            {
                Console.WriteLine("Appointment is not found");
                return null;
            }
        }

        // Not being used.
        public IEnumerable<Appointment> Get()
        {
            throw new NotImplementedException();
        }

        public void Post(Appointment entity)
        {
            try
            {
                checkInContextDB.Appointments.Add(entity);
                checkInContextDB.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Create failed");
            }
        }

        public void Put(Appointment entity)
        {
            try
            {
                checkInContextDB.Appointments.Update(entity);
                checkInContextDB.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Update failed");
            }
        }
    }
}
