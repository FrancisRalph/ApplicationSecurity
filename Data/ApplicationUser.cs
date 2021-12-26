using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ApplicationSecurity.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string CreditCardNumber { get; set; }
        
        public string CreditCardSecurityCode { get; set; } // CVV or CVC
        
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        public string PhotoPath { get; set; }
        
    }
}