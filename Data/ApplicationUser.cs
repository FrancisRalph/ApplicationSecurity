using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ApplicationSecurity.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        // [CreditCard] // don't use validator here as this will be encrypted value
        public string CreditCardNumber { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        public string PhotoPath { get; set; }
        
    }
}