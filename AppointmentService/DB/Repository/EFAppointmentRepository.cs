using AppointmentService.Domain;
using AppointmentService.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.DB.Repository
{
    public class EFAppointmentRepository : IAppointmentRepository
    {

        private readonly AppointmentServiceContext context;
        private readonly AppointmentReadServiceContext readServiceContext;

        public EFAppointmentRepository(AppointmentServiceContext context, AppointmentReadServiceContext readServiceContext)
        {
            this.context = context;
            this.readServiceContext = readServiceContext;
        }

        public AppointmentRead AddAppointment(Appointment appointment)
        {
            context.Appointments.Add(appointment);
            var returnValue = AddReadAppointment(appointment);
            context.SaveChanges();
            return returnValue;

        }

        private AppointmentRead AddReadAppointment(Appointment appointment)
        {

            var appointmentRead = new AppointmentRead()
            {
                AppointmentId = appointment.Id,
                PhysicianId = appointment.Physician.Id,
                GPId = appointment.Patient.GP.Id,
                PatientId = appointment.Patient.Id,
                PreviousAppointmentId = null,
                Name = appointment.Name,
                AppointmentDate = appointment.AppointmentDate,
                PhysicianFirstName = appointment.Physician.FirstName,
                PhysicianLastName = appointment.Physician.LastName,
                PhysicianEmail = appointment.Physician.Email,
                PhysicianRole = appointment.Physician.Role,
                GPFirstName = appointment.Patient.GP.FirstName,
                GPFlastName = appointment.Patient.GP.LastName,
                GPEmail = appointment.Patient.GP.Email,
                PatientFirstName = appointment.Patient.FirstName,
                PatientLastName = appointment.Patient.LastName,
                PatientPhoneNumber = appointment.Patient.PhoneNumber

            };

            if(appointment.PreviousAppointment != null)
            {
                appointmentRead.PreviousAppointmentId = appointment.PreviousAppointment.Id;
            }

            readServiceContext.appointmentsRead.Add(appointmentRead);
            readServiceContext.SaveChanges();
            return appointmentRead;
        }

        private AppointmentRead UpdateReadAppointment (Appointment appointment) { 
            var appointmentToUpdate = GetAppointmentById(appointment.Id);
            appointmentToUpdate.AppointmentId = appointment.Id;
            appointmentToUpdate.PhysicianId = appointment.Physician.Id;
            appointmentToUpdate.GPId = appointment.Patient.GP.Id;
            appointmentToUpdate.PatientId = appointment.Patient.Id;
            appointmentToUpdate.PreviousAppointmentId = null;
            appointmentToUpdate.Name = appointment.Name;
            appointmentToUpdate.AppointmentDate = appointment.AppointmentDate;
            appointmentToUpdate.PhysicianFirstName = appointment.Physician.FirstName;
            appointmentToUpdate.PhysicianLastName = appointment.Physician.LastName;
            appointmentToUpdate.PhysicianEmail = appointment.Physician.Email;
            appointmentToUpdate.PhysicianRole = appointment.Physician.Role;
            appointmentToUpdate.GPFirstName = appointment.Patient.GP.FirstName;
            appointmentToUpdate.GPFlastName = appointment.Patient.GP.LastName;
            appointmentToUpdate.GPEmail = appointment.Patient.GP.Email;
            appointmentToUpdate.PatientFirstName = appointment.Patient.FirstName;
            appointmentToUpdate.PatientLastName = appointment.Patient.LastName;
            appointmentToUpdate.PatientPhoneNumber = appointment.Patient.PhoneNumber;

            if (appointment.PreviousAppointment != null)
            {
                appointmentToUpdate.PreviousAppointmentId = appointment.PreviousAppointment.Id;
            }

            readServiceContext.appointmentsRead.Update(appointmentToUpdate);
            readServiceContext.SaveChanges();
            return appointmentToUpdate;
        }

        private AppointmentRead DeleteReadAppointment(Guid Id) {
            var appointmentToDelete = GetAppointmentById(Id);
            readServiceContext.appointmentsRead.Remove(appointmentToDelete);
            readServiceContext.SaveChanges();
            return appointmentToDelete;
        }

        public AppointmentRead DeleteAppointment(Guid id)
        {
    
            Console.WriteLine(id);
            var appointment = GetWriteAppointmentById(id);
            context.Remove(appointment);
            var deletedAppointment = DeleteReadAppointment(id);
            context.SaveChanges();
            return deletedAppointment;
        }

        public IEnumerable<AppointmentRead> GetAllAppointments()
        {
            return readServiceContext.appointmentsRead;
        }

        public AppointmentRead GetAppointmentById(Guid Id)
        {
            return readServiceContext.appointmentsRead.Where(a => a.AppointmentId.Equals(Id)).FirstOrDefault();
        }

        public AppointmentRead UpdateAppointment(Appointment appointment)
        {
            var edit = GetWriteAppointmentById(appointment.Id);
            edit.Name = appointment.Name;
            edit.AppointmentDate = appointment.AppointmentDate;
            edit.Physician = appointment.Physician;
            edit.Patient = appointment.Patient;
            edit.PreviousAppointment = appointment.PreviousAppointment;

            context.Update(edit);
            //edit = appointment;
            context.SaveChanges();
            return UpdateReadAppointment(appointment);
        }

        public Appointment GetWriteAppointmentById(Guid Id)
        {
            return context.Appointments.Include(c => c.Patient).Include(c => c.Physician).Include(c => c.PreviousAppointment).Include(c => c.Patient.GP).Where(a => a.Id.Equals(Id)).FirstOrDefault();
        }
    }
}
