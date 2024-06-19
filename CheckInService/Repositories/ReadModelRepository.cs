using CheckInService.Models;

namespace CheckInService.Repositories
{
    public class ReadModelRepository
    {

        public ReadModelRepository() {
            Console.WriteLine("Read repository initialized.");
        }

        // Get All
        public IEnumerable<CheckInReadModel> Get()
        {
            return new List<CheckInReadModel>();
        }

        // Get one
        public CheckInReadModel Get(Guid id)
        {
            return new CheckInReadModel();
        }

        // Get one by appointment id
        public CheckInReadModel GetByAppointment(Guid id)
        {
            return new CheckInReadModel();
        }

        // Update appointment

        // Update checkin part.

        // Delete appointment
        public void DeleteByAppointment(Guid id)
        {
            Console.WriteLine($"{id} deleted");
        }

        // Delete all.
        public void DeleteAll()
        {
            Console.WriteLine($"All has been lost!");
        }
    }
}
