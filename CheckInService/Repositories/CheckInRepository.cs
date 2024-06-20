using CheckInService.DBContexts;
using CheckInService.Models;
using CheckInService.Models.Queries;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.EntityFrameworkCore;

namespace CheckInService.Repositories
{
    public class CheckInRepository
    {
        private readonly CheckInContextDB checkInContextDB;

        public CheckInRepository(CheckInContextDB checkInContextDB)
        {
            this.checkInContextDB = checkInContextDB;
        }

        // Will be most likely be served via events
        public void Post(CheckIn checkIn)
        {
            try
            {
                checkInContextDB.Add(checkIn);
                checkInContextDB.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Create failed");
            }
            
        }

        public void Put(CheckIn checkIn)
        {
            try
            {
                checkInContextDB.Update(checkIn);
                checkInContextDB.SaveChanges(true);
            }
            catch
            {
                Console.WriteLine("Update failed.");
            }
        }

        public IEnumerable<CheckInView> Get()
        {
            return checkInContextDB.checkInsView.AsEnumerable();
        }

        public IEnumerable<CheckIn> GetCheckIns()
        {
            return checkInContextDB.checkIns
                .Include(ap => ap.Appointment.Physician)
                .Include(app => app.Appointment.Patient)
                .ToList();
        }

        public CheckIn? Get(Guid serialNumber)
        {
            try
            {
                var jj = checkInContextDB.checkIns.
                Include(p => p.Appointment.Physician).
                Include(ppp => ppp.Appointment.Patient)
                .Where(Patient => Patient.SerialNr.Equals(serialNumber)).First();
                return jj;
            }
            catch
            {
                return null;
            }
            
        }

        public CheckInView GetView(int id)
        {
            return checkInContextDB.checkInsView.Find(id);
        }
    }
}
