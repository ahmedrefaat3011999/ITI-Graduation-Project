using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.ViewModel
{
    public class UserRegisterVM
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        [Remote("IsEmailAvailable", "Account", ErrorMessage = "The email already is  token.")]
        public string Email { get; set; }
        //[RegularExpression(@"^(00201|\\+201|01)[0-2,5]{1}[0-9]{8}$",ErrorMessage ="phone number not valid format")]
        public List<string>? PhoneNumbers { get; set; }
        [Required(ErrorMessage ="Enter Your Age")]
        public int? Age { get; set; }
        [Required(ErrorMessage ="Gender Required")]
        public string? Gender { get; set; }
        public string? ClinicId { get; set; }
        [Required(ErrorMessage ="Image Required" )]
        //[DataType(DataType.Upload)]
        [NotMapped]
       //[RegularExpression(@"[^\\s]+(.*?)\\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$", ErrorMessage = "Only jpg, jpeg, png, and gif file types are allowed.")]
        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
    
        public string? Country { get; set; }
     

        public string? City { get; set; }
       
        public string? Region { get; set; }
        public List<string>? SpecialistDoctors { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Enter complex password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Matched with Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Enter Your Role")]
        public string RoleName { get; set; }
    }
}
