using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.JwtAthentication.Models
{
    public class RegistrationModel
    {
        private const string emailRegex =
                @"^[a-zA-Z]{3,}([a-zA-Z0-9.]{3,}){0,}@[a-zA-Z]{2,}([a-zA-Z0-9.]{3,}){0,}.com$";


        [MinLength(3)]
        [DataType(DataType.Text)]
        public string Name { get; set; }


        [Range(10, 50)]
        public int Age { get; set; }


        [MinLength(3)]
        [DataType(DataType.Text)]
        public string Address { get; set; }


        [RegularExpression(emailRegex, 
            ErrorMessage = "The Email must be in this form user@example.com")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


        [MinLength(3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [MinLength(3)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Matched")]
        public string ConfirmPassword { get; set; }
    }
}
