using PatientService.Domain;
using System.Diagnostics;

namespace PatientService.Data
{
    public class DbInitializer
    {
        public static void Initialize(PatientDBContext context)
        {
            // Look for any students.
            if (context.Patients.Any())
            {
                return;   // DB has been seeded
            }

            var gps = new GeneralPractitioner[]
            {
                new GeneralPractitioner{FirstName="Billy", LastName="Ooglap"},
            };

            context.GeneralPractitioners.AddRange(gps);
            context.SaveChanges();

            var patients = new Patient[]
            {
                new() {FirstName="Carson",LastName="Alexander", DateOfBirth=DateOnly.Parse("2019-09-01"), Email="Caalex@mail.com", BSN="", GeneralPractitioner=gps.First()},
            };

            context.Patients.AddRange(patients);
            context.SaveChanges();
        }
    }
}
