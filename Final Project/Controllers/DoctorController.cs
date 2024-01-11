using Final_Project.Models.DataContext;
using Final_Project.Models.DomainModels;
using Final_Project.Repositary;
using Final_Project.ViewModel;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Final_Project.Controllers
{
    public class DoctorController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly UserRepositry userRepositry;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DataContext db;

        public DoctorController(SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> roleManager,
            DataContext _db, UserManager<ApplicationUser> userManager, UserRepositry _userRepositry)
        {

            this.signInManager = _signInManager;
            this.roleManager = roleManager;
            db = _db;
            this.userManager = userManager;
            userRepositry = _userRepositry;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detailes(string id)
        {
            var users = await userManager.GetUsersInRoleAsync("Doctor");
            var doctor = users.Where(d => d.Id == id).FirstOrDefault();

            ViewBag.specialDoctor = db.DoctorSpecialists.FirstOrDefault(d => d.DoctorId == doctor.Id).SpecialName;
            ViewBag.DoctorPhones = db.PhoneUsers.FirstOrDefault(d => d.UserId == doctor.Id).PhoneNumber;

            return View(doctor);
        }


        [HttpGet]
        public async Task<IActionResult> searchDoctor()
        {
            var users = await userManager.GetUsersInRoleAsync("Doctor");
            ViewBag.AllDoctors = users;
            ViewBag.AllClinics = db.Clinics.ToList();
            ViewBag.AllSpecialist = db.DoctorSpecialists.ToList();
            List<UserRegisterVM> doctors = new List<UserRegisterVM>();
            foreach (var doc in users)
            {
                doctors.Add(new UserRegisterVM()
                {
                    Id = doc.Id,
                    UserName = doc.UserName,
                    //PhoneNumber = doc.PhoneNumber,
                    Email = doc.Email,
                    SpecialistDoctors = db.DoctorSpecialists.Where(d => d.DoctorId == doc.Id).Select(d => d.SpecialName).ToList(),
                    Gender = doc.Gender,
                    Age = doc.Age,
                    City = doc.City,
                    Country = doc.Country,
                    Region = doc.Region,

                    ImageName = doc.ImageName

                });
            }
            return View(doctors);

        }

        [HttpPost]
        public async Task<IActionResult> searchDoctor(string doctorName, string region, string specialName)
        {
            // Start with a base query that includes the necessary navigation properties
            var searchQuery = db.Users.Where(d => d.DoctorSpecialists.Count() != 0).Include(d => d.DoctorSpecialists).Include(user => user.Clinic).ToList();

            // Apply filters based on the provided parameters
            if (!string.IsNullOrEmpty(doctorName))
            {
                searchQuery = searchQuery.Where(u => u.UserName.Contains(doctorName)).ToList();
            }

            if (!string.IsNullOrEmpty(region))
            {
                searchQuery = searchQuery.Where(u => u.Clinic.City.Contains(region)).ToList();
            }

            if (!string.IsNullOrEmpty(specialName))
            {
                searchQuery = searchQuery.Where(u => u.DoctorSpecialists.Any(ds => ds.SpecialName.Contains(specialName))).ToList();
            }

            // Execute the query and retrieve the results
            var searchResults = searchQuery.ToList();

            // You can now use the searchResults as needed, for example, pass it to a view

            List<UserRegisterVM> doctors = new List<UserRegisterVM>();
            foreach (var doc in searchResults)
            {
                // var users = await userManager.GetUsersInRoleAsync("Doctor");
                doctors.Add(new UserRegisterVM()
                {
                    Id = doc.Id,
                    UserName = doc.UserName,
                    //PhoneNumber = doc.PhoneNumber,
                    Email = doc.Email,
                    SpecialistDoctors = db.DoctorSpecialists.Where(d => d.DoctorId == doc.Id).Select(d => d.SpecialName).ToList(),
                    Gender = doc.Gender,
                    Age = doc.Age,
                    City = doc.City,
                    Country = doc.Country,
                    Region = doc.Region,
                    ImageName = doc.ImageName
                });

            }
            var users = await userManager.GetUsersInRoleAsync("Doctor");
            ViewBag.AllDoctors = users;

            ViewBag.AllClinics = db.Clinics.ToList();
            ViewBag.AllSpecialist = db.DoctorSpecialists.ToList();

            return View("searchDoctor", doctors);


        }

        [HttpGet]
        [Authorize(Roles ="Doctor")]
        public IActionResult ShowAllPatientforDoctor()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var AllAppointment = db.Appointments.Where(p=>p.DoctorId== userId).ToList();
            if(userId !=null && AllAppointment != null)
            {
                
                List<PatientAppointmentsVM> PatientsAppoints = new List<PatientAppointmentsVM>();
                foreach (var item in AllAppointment)
                {
                    
                    var patient = db.Users.Where(u => u.Id == item.PatientId).FirstOrDefault();
                    if (patient != null)
                    {
                        PatientAppointmentsVM patientAppoint=new PatientAppointmentsVM()
                        {
                            UserName = patient.UserName,
                            PhoneNumbers = db.PhoneUsers.Where(p => p.UserId == patient.Id).Select(p => p.PhoneNumber).ToList(),
                            Email = patient.Email,
                            Gender = patient.Gender,
                            Age = patient.Age,
                            ImageName = patient.ImageName,
                            DateReserved = item.DateReserved,
                            TimeReserved=item.TimeReserved,
                            

                        };
                        PatientsAppoints.Add(patientAppoint);

                    }

                }
                return View(PatientsAppoints);

            }
            return NotFound();

        }




    }

}
