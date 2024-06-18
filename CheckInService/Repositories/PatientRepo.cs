using CheckInService.DBContexts;
using CheckInService.Models;

namespace CheckInService.Repositories
{
    public class PatientRepo : IRepo<Patient>
    {
        private readonly CheckInContextDB checkInContextDB;

        public PatientRepo(CheckInContextDB checkInContextDB)
        {
            this.checkInContextDB = checkInContextDB;
        }

        public IEnumerable<Patient> Get()
        {
            throw new NotImplementedException();
        }

        public Patient? Get(int id)
        {
            return checkInContextDB.Patients.Find(id);
        }

        public Patient? Get(Guid guid)
        {
            try
            {
                return checkInContextDB.Patients.Where(p => p.PatientSerialNr.Equals(guid)).First();
            }
            catch
            {
                Console.WriteLine("Patient is not found");
                return null;
            }
        }

        public void Post(Patient entity)
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

        public void Put(Patient entity)
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

        public void Delete(Guid guid)
        {

        }
    }
}
