using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportPatientService
{
    public class Patient
    {
        [Name("Company Name")]
        public string CompanyName { get; set; }
        [Name("First Name")]
        public string FirstName { get; set; }
        [Name("Last Name")]
        public string LastName { get; set; }
        [Name("Phone Number")]
        public string PhoneNumber { get; set; }
        [Name("Address")]
        public string Address { get; set; }
        public static Patient FromCSV(string csvLine)
        {

            string[] values = csvLine.Split(',');
            return new Patient() { FirstName = values[1], LastName = values[2], PhoneNumber = values[3], Address = values[4] };
        }
    }

}
