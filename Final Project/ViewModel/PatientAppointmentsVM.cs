using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.ViewModel
{
    public class PatientAppointmentsVM
    {
        public int Id { get; set; }
        public string doctorId { get; set; }
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        [Remote("IsEmailAvailable", "Account", ErrorMessage = "The email address is already in use.")]
        public string Email { get; set; }
        public List<string>? PhoneNumbers { get; set; }
        [Required(ErrorMessage ="Enter Your Age")]
        public int? Age { get; set; }
        [Required(ErrorMessage ="Gender Required")]
        public string? Gender { get; set; }
        [Required(ErrorMessage ="Image Required" )]
        //[DataType(DataType.Upload)]
        [NotMapped]
        [RegularExpression(@"^.*\.(jpg|jpeg|png|gif)$", ErrorMessage = "Only jpg, jpeg, png, and gif file types are allowed.")]
        public IFormFile Image { get; set; }
        public string? ImageName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateReserved { get; set; }
        [DataType(DataType.Time)]
        public DateTime? TimeReserved { get; set; }
    }
}
