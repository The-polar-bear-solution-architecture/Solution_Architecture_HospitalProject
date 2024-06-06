using CheckInService.Models;

namespace CheckInService.Repositories
{
    public class CheckInRepository
    {

        public CheckInRepository()
        {
        }

        public void Post(CheckIn checkIn)
        {

        }

        public void Put(CheckIn checkIn)
        {

        }

        public IEnumerable<CheckIn> Get()
        {
            return new List<CheckIn>();
        }

        public CheckIn Get(int id)
        {
            return new CheckIn { Id = id };
        }
    }
}
