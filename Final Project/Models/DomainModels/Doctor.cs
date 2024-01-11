using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models.DomainModels
{
    public class Doctor: ApplicationUser
    {
        //public string Id { get; set; }
        //public string? Name { get; set; }
        //public string? Email { get; set; }
      
       
        public virtual List<PhoneUser>? PhoneDoctors { get; set; }
       // public virtual List<Appointment>? Appointments { get; set; }
       // public virtual List<Doctor_patient>? Doctor_Patients { get; set; }
      
    }
}
