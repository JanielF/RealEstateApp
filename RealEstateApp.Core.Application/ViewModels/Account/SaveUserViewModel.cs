using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Account
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "You must enter the email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter the username.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter the firstname.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter the lastname.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter the Document Id.")]
        [DataType(DataType.Text)]
        public string DocumentId { get; set; }

        [Required(ErrorMessage = "You must enter the phone number.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string? UserImagePath { get; set; }

        public IFormFile? UserImage { get; set; }

        //[Required(ErrorMessage = "You must enter the password.")]
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.[a-z])(?=.[A-Z])(?=.\d)(?=.\W).+$", ErrorMessage = "Must contain one uppercase character, one lowercase character, one digit and one non-alphanumeric character. At least six characters long.")]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        //[Required(ErrorMessage = "You must enter the password.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public string Role { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }


    
    }
}
