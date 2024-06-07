using CheckInService.DBContexts;
using CheckInService.Models;

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
            
            Console.WriteLine("Create the checkin");
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

        public IEnumerable<CheckIn> Get()
        {
            return checkInContextDB.checkIns.AsEnumerable();
        }

        public CheckIn? Get(int id)
        {
            return checkInContextDB.checkIns.Find(id);
        }
    }
}
