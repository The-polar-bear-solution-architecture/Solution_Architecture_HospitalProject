using AppointmentService.Domain;

namespace AppointmentService.DomainServices
{
    public interface IGeneralPractitionerRepository
    {
        public GeneralPractitioner AddPractitioner(GeneralPractitioner generalPractitioner);

        public GeneralPractitioner RemovePractitioner(Guid Id);

        public GeneralPractitioner UpdatePractitioner(GeneralPractitioner generalPractitioner);

        public GeneralPractitioner GetPractitionerById(Guid Id);
    }
}
