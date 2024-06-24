using AppointmentService.CommandsAndEvents.Events;
using AppointmentService.Domain;
using AppointmentService.DomainServices;

namespace AppointmentService.CommandsAndEvents.Commands
{
    public class PatientCommandHandler
    {
        private IPatientRepository _patientRepository;
        private readonly IGeneralPractitionerRepository _generalPractitionerRepository;

        public PatientCommandHandler(IPatientRepository patientRepository, IGeneralPractitionerRepository generalPractitionerRepository)
        {
            _patientRepository = patientRepository;
            _generalPractitionerRepository = generalPractitionerRepository;
        }

        public void PatientCreated(PatientCreated createdPatient) {
            var gp = _generalPractitionerRepository.GetPractitionerById(createdPatient.GeneralPractitioner.Id);
            if (gp == null)
            {
                _generalPractitionerRepository.AddPractitioner(createdPatient.GeneralPractitioner);
            }
            var patient = new Patient()
            {
                Id = createdPatient.Id,
                FirstName = createdPatient.FirstName,   
                LastName = createdPatient.LastName,
                PhoneNumber = createdPatient.PhoneNumber,
                GP = gp
            };



            _patientRepository.AddPatient(patient);
        }  

        public void PatientUpdated(PatientUpdated updatedPatient)
        {
            var gp = _generalPractitionerRepository.GetPractitionerById(updatedPatient.GeneralPractitioner.Id);
            var patientToUpdate = _patientRepository.GetPatientById(updatedPatient.Id); 
            patientToUpdate.FirstName = updatedPatient.FirstName;
            patientToUpdate.LastName = updatedPatient.LastName;
            patientToUpdate.PhoneNumber = updatedPatient.PhoneNumber;
            patientToUpdate.GP = gp;

            _patientRepository.UpdatePatient(patientToUpdate);
        }

        public async void PatientDeleted(PatientDeleted deletedPatient)
        {
            await _patientRepository.DeletePatient(deletedPatient.Id);
        }



    }
}
