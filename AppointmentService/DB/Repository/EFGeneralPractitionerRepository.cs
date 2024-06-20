using AppointmentService.Domain;
using AppointmentService.DomainServices;
using Polly;

namespace AppointmentService.DB.Repository
{
    public class EFGeneralPractitionerRepository : IGeneralPractitionerRepository
    {

        private readonly AppointmentServiceContext _context;

        public EFGeneralPractitionerRepository(AppointmentServiceContext context)
        {
            _context = context;
        }

        public GeneralPractitioner AddPractitioner(GeneralPractitioner generalPractitioner)
        {
            _context.GeneralPractitioners.Add(generalPractitioner);
            _context.SaveChanges();
            return generalPractitioner; 
        }

        public GeneralPractitioner GetPractitionerById(Guid Id)
        {
            return _context.GeneralPractitioners.Where(gp => gp.Id.Equals(Id)).FirstOrDefault();
        }

        public GeneralPractitioner RemovePractitioner(Guid Id)
        {
            var gp = GetPractitionerById(Id);
            _context.Remove(gp);
            _context.SaveChanges();
            return gp;
        }

        public GeneralPractitioner UpdatePractitioner(GeneralPractitioner generalPractitioner)
        {
            _context.Update(generalPractitioner);
            _context.SaveChanges();
            return generalPractitioner;
        }
    }
}
