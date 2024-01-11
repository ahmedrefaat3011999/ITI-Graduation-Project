using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModel
{
    public class AppointmentVm
    {
        public string? Id { get; set; }
        //public string DoctorId { get; set; }
        //public string PatientId { get; set; }
        //public string clinicId { get; set; }
        public string? PatientName { get; set; }
        // [Required(ErrorMessage ="Enter your Email")]

        [Required]
        //[EmailAddress]
       // [Remote("IsEmailAvailable", "Appointment", ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateReserved { get; set; }
        [DataType(DataType.Time)]
        public DateTime TimeReserved { get; set; }
        public string? Description { get; set; }



    }
}
