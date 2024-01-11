using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models.DomainModels
{
    public class Appointment
    {
        public int Id { get; set; }
        public string? PatientName { get; set; }
        public string? PhoneNumber { get; set; }
        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }
        public DateTime? DateReserved { get; set; }
        [DataType(DataType.Time)]
        public DateTime? TimeReserved { get; set; }
        //public string? AppointPrice { get; set; }
        public string? AppointState { get; set; }
        public string? Description { get; set; }
        [ForeignKey(nameof(Patient))]
        public string? PatientId { get; set; }
        [InverseProperty("AppointmentsPatients")]
        public virtual ApplicationUser? Patient  { get; set; }
        [ForeignKey(nameof(Doctor))]
        public string? DoctorId { get; set; }
        [InverseProperty("AppointmentsDoctors")]
        public virtual ApplicationUser? Doctor { get; set; }
        [ForeignKey(nameof(Clinic))]
        public string? ClinicId { get; set; }
        public virtual Clinic? Clinic { get; set; }

    }
}
