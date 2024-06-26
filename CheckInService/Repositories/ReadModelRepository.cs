using CheckInService.CommandsAndEvents.Commands.Appointment;
using CheckInService.CommandsAndEvents.Commands.CheckIn;
using CheckInService.DBContexts;
using CheckInService.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckInService.Repositories
{
    public class ReadModelRepository
    {
        private readonly CheckInReadContextDB contextDB;

        public ReadModelRepository(CheckInReadContextDB contextDB) {
            Console.WriteLine("Read repository initialized.");
            this.contextDB = contextDB;
        }

        // Get All
        public IEnumerable<CheckInReadModel> Get()
        {
            return contextDB.CheckInReadModel.ToList();
        }

        // Get one
        public CheckInReadModel Get(Guid id)
        {
            try
            {
                return contextDB.CheckInReadModel.Where(e => e.CheckInSerialNr.Equals(id)).First();
            }
            catch
            {
                return null;
            }
        }

        // Get one by appointment id
        public CheckInReadModel GetByAppointment(Guid id)
        {
            return contextDB.CheckInReadModel.Where(e => e.AppointmentGuid.Equals(id)).First();
        }

        public List<CheckInReadModel>GetByPatient(Guid patientId)
        {
            return contextDB.CheckInReadModel.Where(e => e.PatientGuid.Equals(patientId)).ToList();
        }

        public CheckInReadModel Create(CheckInReadModel model)
        {
            try
            {
                contextDB.CheckInReadModel.Add(model);
                contextDB.SaveChanges();
                return model;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<CheckInReadModel> BulkCreate(IEnumerable<CheckInReadModel> list)
        {
            try
            {
                foreach (var item in list)
                {
                    contextDB.CheckInReadModel.Add(item);
                    Console.WriteLine("Addded record");
                    contextDB.SaveChanges();
                    
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        public void BulkUpdate(List<CheckInReadModel> checkInReadModels)
        {
            try
            {
                contextDB.UpdateRange(checkInReadModels);
                contextDB.SaveChanges(true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        // Update appointment
        public CheckInReadModel Update(AppointmentUpdateCommand model)
        {
            try
            {
                CheckInReadModel m = GetByAppointment(model.AppointmentSerialNr);
                if(m != null)
                {
                    m.ApointmentName = model.AppointmentName;
                    m.AppointmentDate = model.AppointmentDate;
                    // Extra data needed regarding physicians data.
                    if (!model.PhysicianSerialNr.Equals(m.PhysicianGuid))
                    {
                        m.PhysicianGuid = model.PhysicianSerialNr;
                        m.PhysicianLastName = model.PhysicianLastName;
                        m.PhysicianEmail = model.PhysicianEmail;
                        m.PhysicianFirstName = model.PhysicianFirstName;
                    }
                    contextDB.Update(m);
                    contextDB.SaveChanges();
                }
                return m;
            }
            catch
            {
                return null;
            }
        }

        // Update checkin part.
        public CheckInReadModel Update(CheckInUpdateCommand model)
        {
            try
            {
                CheckInReadModel m = Get(model.CheckInSerialNr);
                if (m != null)
                {
                    m.Status = model.Status;
                    contextDB.Update(m);
                    contextDB.SaveChanges();
                }
                return m;
            }
            catch
            {
                return null;
            }
        }

        // Delete appointment
        public void DeleteByAppointment(Guid id)
        {
            try
            {
                var entity = GetByAppointment(id);
                contextDB.CheckInReadModel.Remove(entity);
                int i = contextDB.SaveChanges(true);

                Console.WriteLine($"Deletion rows {i}");
            }
            catch
            {
                Console.WriteLine("Nothing found");
            }
        }

        // Delete all.
        public void DeleteAll()
        {
            // Clears database.
            var l = contextDB.CheckInReadModel.ToList();
            contextDB.CheckInReadModel.RemoveRange(l);
            contextDB.SaveChanges();
        }
    }
}
