using CheckinService.Model;
using CheckInService.DBContexts;

namespace CheckInService.Repositories
{
    public class PhysicianRepo : IRepo<Physician>
    {
        private readonly CheckInContextDB checkInContextDB;

        public PhysicianRepo(CheckInContextDB checkInContextDB)
        {
            this.checkInContextDB = checkInContextDB;
        }

        public IEnumerable<Physician> Get()
        {
            throw new NotImplementedException();
        }

        public Physician? Get(int id)
        {
            return checkInContextDB.Physicians.Find(id);
        }

        public void Post(Physician entity)
        {
            try
            {
                checkInContextDB.Add(entity);
                checkInContextDB.SaveChangesAsync().Wait();
            }
            catch
            {
                Console.WriteLine("");
            }
        }

        public void Put(Physician entity)
        {
            try
            {
                checkInContextDB.Update(entity);
                checkInContextDB.SaveChangesAsync().Wait();
            }
            catch
            {
                Console.WriteLine("");
            }
        }
    }
}
