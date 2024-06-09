using CheckInService.DBContexts;
using CheckInService.Models;
using CheckInService.Queries;

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
            return checkInContextDB.checkIns.Find(id);
        }
    }
}
