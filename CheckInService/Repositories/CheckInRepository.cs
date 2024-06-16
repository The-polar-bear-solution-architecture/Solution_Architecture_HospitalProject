using CheckInService.DBContexts;
using CheckInService.Models;
using CheckInService.Queries;
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
                checkInContextDB.SaveChangesAsync().Wait();
            }
            catch
            {
                Console.WriteLine("");
            }
        }

        public IEnumerable<CheckInView> Get()
        {
            return checkInContextDB.checkInsView.AsEnumerable();
        }

        public CheckIn? Get(int id)
        {
            var jj = checkInContextDB.checkIns.
                Include(p => p.Appointment.Physician).
                Include(ppp => ppp.Appointment.Patient)
                .Where(Patient => Patient.Id == id).First();
            return jj;
        }

        public CheckInView GetView(int id)
        {
            return checkInContextDB.checkInsView.Find(id);
        }
    }
}
