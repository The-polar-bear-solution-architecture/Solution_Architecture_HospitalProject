using System.ComponentModel.DataAnnotations;

namespace PatientService.Domain
{
    public class Patient
    {
        [Required]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public GeneralPractitioner? GeneralPractitioner{ get; set; }

    }
}
