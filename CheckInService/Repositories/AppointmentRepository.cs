using CheckinService.Model;
using CheckInService.DBContexts;
using CheckInService.Models;

namespace CheckInService.Repositories
{
    public class AppointmentRepository: IRepo<Appointment>
    {
        private readonly CheckInContextDB checkInContextDB;

        public AppointmentRepository(CheckInContextDB checkInContextDB)
        {
            this.checkInContextDB = checkInContextDB;
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

        /* public Appointment? Get(string guid)
        {
            try
            {
                return checkInContextDB.Appointments.Where(p => p.AppointmentGuid == guid).First();
            }
            catch
            {
                return null;
            }
        } */

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
