using Final_Project.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Models.DataContext
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        
        public DataContext() : base()
        {

        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> patients { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
      //  public virtual DbSet<Doctor_patient> Doctor_Patients { get; set; }
        public virtual DbSet<PhoneUser> PhoneUsers { get; set; }
        public virtual DbSet<DoctorSpecialist> DoctorSpecialists { get; set; }
      
        //public virtual DbSet<Clinic_patient> Clinic_Patients { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Clinic_patient>()
            // .HasKey(m => new { m.CinicId, m.PatientId });
            //modelBuilder.Entity<Doctor_patient>()
            //.HasKey(m => new { m.DoctorId, m.PatientId });

            //modelBuilder.Entity<Doctor_patient>()
            //.HasOne(dp => dp.Patient)
            //.WithMany(p => p.Doctor_Patients)
            //.HasForeignKey(dp => dp.PatientId)
            //.OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Doctor", NormalizedName = "DOCTOR" },
                new IdentityRole { Id = "3", Name = "Patient", NormalizedName = "PATIENT" }
               );
        }
    }
}
