using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models.DomainModels
{
    public class Clinic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        //[Key]
        //public string? Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Enter Clinic Name")]
        public string Name { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        [DataType(DataType.Time)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Time)]
        public DateTime? EndDate { get; set; }
        public string? PhoneClinic { get; set; }
        public virtual List<Appointment>? Appointments { get; set; }
      //  public virtual List<Clinic_patient>? Clinic_Patients { get; set; }
        //public virtual List<ApplicationUser>? Doctors { get; set; }

    }

}
