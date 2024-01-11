using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models.DomainModels
{
    public class ApplicationUser:IdentityUser
    {
        public string? Gender { get; set; }
     
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public int? Age { get; set; }
        public string? ImageName { get; set; }
        ////public int? PhoneNumber { get; set; }
        [ForeignKey(nameof(Clinic))]
        public string? ClinicId { get; set; }
        public virtual Clinic? Clinic { get; set; }
        public virtual List<DoctorSpecialist>? DoctorSpecialists { get; set; }
        public virtual List<PhoneUser>? Phones { get; set; }
       
        public virtual ICollection<Appointment>? AppointmentsPatients { get; set; }
      
        public virtual ICollection<Appointment>? AppointmentsDoctors { get; set; }


    }
}
