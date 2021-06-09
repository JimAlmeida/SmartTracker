using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTracker.Models.DTOs
{
    public class UserModel
    {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateOrProvince {get; set; }
        public string Country { get; set; }
        public string AboutMe { get; set; }
        public string JobRole { get; set; }
        public string JobDescription { get; set; }
        public string ImageURL { get; set; }
        public string ImageData { get; set; }
    }
}
