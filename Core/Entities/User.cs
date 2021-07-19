using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class User : BaseEntity
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


        [RegularExpression(emailRegex)]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Remote(action: "IsUniqueEmailServerValidation", 
                controller: "Registration",
                AdditionalFields = nameof(Email),
                ErrorMessage = "Email is already existing")]
        public string Email { get; set; }


        [MinLength(3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [MinLength(3)]
        [JsonIgnore]
        [DataType(DataType.Password), NotMapped]
        [Compare("Password", ErrorMessage = "Password Not Matched")]
        public string ConfirmPassword { get; set; }
    }
}
