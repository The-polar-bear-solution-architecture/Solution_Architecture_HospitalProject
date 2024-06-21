using AppointmentService.CommandsAndEvents.Events;
using AppointmentService.Domain;
using AppointmentService.DomainServices;

namespace AppointmentService.CommandsAndEvents.Commands
{
    public class PatientCommandHandler
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IGeneralPractitionerRepository _generalPractitionerRepository;

        public PatientCommandHandler(IPatientRepository patientRepository, IGeneralPractitionerRepository generalPractitionerRepository)
        {
            _patientRepository = patientRepository;
            _generalPractitionerRepository = generalPractitionerRepository;
        }

        public void PatientCreated(PatientCreated createdPatient) {
            var gp = _generalPractitionerRepository.GetPractitionerById(createdPatient.GPId);
            var patient = new Patient()
            {
                Id = createdPatient.PatientID,
                FirstName = createdPatient.FirstName,   
                LastName = createdPatient.LastName,
                PhoneNumber = createdPatient.PhoneNumber,
                GP = gp
            };

            _patientRepository.AddPatient(patient);
        }  

        public void PatientUpdated(PatientUpdated updatedPatient)
        {
            var gp = _generalPractitionerRepository.GetPractitionerById(updatedPatient.GPId);
            var patient = new Patient()
            {
                Id = updatedPatient.PatientID,
                FirstName = updatedPatient.FirstName,
                LastName = updatedPatient.LastName,
                PhoneNumber = updatedPatient.PhoneNumber,
                GP = gp
            };

            _patientRepository.UpdatePatient(patient);
        }

        public void PatientDeleted(PatientDeleted deletedPatient)
        {
            _patientRepository.DeletePatient(deletedPatient.Id);
        }



    }
}
