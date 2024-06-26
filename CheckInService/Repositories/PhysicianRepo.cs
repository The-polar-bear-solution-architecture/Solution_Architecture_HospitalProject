﻿using CheckinService.Model;
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

        public Physician? Get(Guid guid)
        {
            try
            {
                return checkInContextDB.Physicians.Where(p => p.PhysicianSerialNr.Equals(guid)).First();
            }
            catch
            {
                Console.WriteLine("Physician is not found");
                return null;
            }
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
                checkInContextDB.SaveChanges();
            }
            catch
            {
                Console.WriteLine("");
            }
        }

        public void Delete(Guid entity)
        {

        }
    }
}
