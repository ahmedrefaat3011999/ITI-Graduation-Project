using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models.DomainModels
{
    public class DoctorSpecialist
    {
        public int Id { get; set; }
        public string? SpecialName  { get; set; }
        [ForeignKey(nameof(Doctor))]
        public string? DoctorId { get; set; }
        public virtual ApplicationUser? Doctor { get; set; }
    }
}
