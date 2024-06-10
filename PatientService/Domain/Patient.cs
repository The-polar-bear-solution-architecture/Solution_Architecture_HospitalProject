namespace PatientService.Domain
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Email { get; set; }
        public string BSN { get; set; }
        public GeneralPractitioner GeneralPractitioner{ get; set; }

    }
}
