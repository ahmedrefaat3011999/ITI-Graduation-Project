using Final_Project.Models.DataContext;
using Final_Project.Models.DomainModels;
using Final_Project.Repositary;
using Final_Project.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace Final_Project.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly DataContext db;
        private readonly UserRepositry userRepositry;

        public AppointmentController(UserManager<ApplicationUser> userManager, DataContext _db ,UserRepositry _userRepositry)
        {
            this.userManager = userManager;
            db = _db;
            this.userRepositry = _userRepositry;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> AddAppoint(AppointmentVm newAppoint, string Id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser Doctor = await userManager.FindByIdAsync(Id);
                ApplicationUser Patient = await userManager.FindByEmailAsync(newAppoint.Email);
                var clinic = db.Clinics.Find(Doctor.ClinicId);
                if (Patient == null || Doctor == null || clinic == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                Appointment appointment = new Appointment()
                {
                    PatientName = newAppoint.PatientName,
                    PhoneNumber = newAppoint.PhoneNumber,
                    DoctorId = Doctor.Id,
                    PatientId = Patient.Id,
                    Description = newAppoint.Description,
                    DateReserved = newAppoint.DateReserved,
                    TimeReserved = newAppoint.TimeReserved,
                    ClinicId = clinic.Id

                };
                db.Appointments.Add(appointment);
                db.SaveChanges();
                string messageBody = newAppoint.DateReserved.ToString("yyyy-MM-dd") +
                    " in hour " + newAppoint.TimeReserved.ToString("HH:mm");
                userRepositry.SendMessage(Patient.Email, $"Appointment Reserved Successfully in doctor {Doctor.UserName} ",
                   messageBody);
                return RedirectToAction("PatientAppointment", "Appointment");
            }
            else
            {
                ModelState.AddModelError("", "email is required");
                return RedirectToAction("searchDoctor", "Doctor");
            }

        }
        [HttpGet]
        [Authorize(Roles = "Patient")]
        public IActionResult PatientAppointment()
        {
            ViewBag.success = TempData["success"];
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var AllAppointment = db.Appointments.Where(p => p.PatientId == userId).ToList();
            if (userId != null && AllAppointment.Count() > 0)
            {
                List<PatientAppointmentsVM> PatientsAppoints = new List<PatientAppointmentsVM>();
                foreach (var item in AllAppointment)
                {

                    var Doctor = db.Users.Where(u => u.Id == item.DoctorId).FirstOrDefault();
                    if (Doctor != null)
                    {
                        PatientAppointmentsVM patientAppoint = new PatientAppointmentsVM()
                        {
                            Id = item.Id,
                            doctorId = item.DoctorId,
                            UserName = Doctor.UserName,
                            PhoneNumbers = db.PhoneUsers.Where(p => p.UserId == Doctor.Id).Select(p => p.PhoneNumber).ToList(),
                            Email = Doctor.Email,
                            Gender = Doctor.Gender,
                            Age = Doctor.Age,
                            ImageName = Doctor.ImageName,
                            DateReserved = item.DateReserved,
                            TimeReserved = item.TimeReserved,


                        };
                        PatientsAppoints.Add(patientAppoint);

                    }

                }
                return View(PatientsAppoints);

            }

            return View(new List<PatientAppointmentsVM>());
        }



        public IActionResult DeleteAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("PatientAppointment");
        }
        [HttpGet]

        public async Task<IActionResult> EditAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(int id, AppointmentVm editappoint)
        {
            var appointment = db.Appointments.Find(id);

            if (ModelState.IsValid)
            {
                appointment.PatientName = editappoint.PatientName;
                appointment.PhoneNumber = editappoint.PhoneNumber;
                appointment.Description = editappoint.Description;
                appointment.DateReserved = editappoint.DateReserved;
                appointment.TimeReserved = editappoint.TimeReserved;
                db.SaveChanges();
                //  TempData["success"] = $"Appointment Edit Successfully in doctor {Doctor.UserName} ";

                return RedirectToAction("PatientAppointment", "Appointment");
            }
            else
            {
                ModelState.AddModelError("", "email is required");
                return RedirectToAction("searchDoctor", "Doctor");
            }



        }


        public async Task<JsonResult> IsEmailAvailable(string Email)
        {
            // Check if the email address is already in use
            bool emailExists = await userManager.Users.AnyAsync(u => u.Email == Email);

                return Json(emailExists);
        }
    }
}
