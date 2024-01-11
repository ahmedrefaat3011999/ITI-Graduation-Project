using Final_Project.Models;
using Final_Project.Models.DataContext;
using Final_Project.Models.DomainModels;
using Final_Project.Repositary;
using Final_Project.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserRepositry userRepositry;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly DataContext db;

        public HomeController(ILogger<HomeController> logger, UserRepositry userRepositry, UserManager<ApplicationUser> userManager, DataContext _db)
        {
            _logger = logger;
            this.userRepositry = userRepositry;
            this.userManager = userManager;
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            var users = await userManager.GetUsersInRoleAsync("Doctor");
            List<UserRegisterVM> doctors = new List<UserRegisterVM>();
            foreach (var doc in users)
            {
                doctors.Add(new UserRegisterVM()
                {
                    Id= doc.Id,
                    UserName = doc.UserName,
                    PhoneNumbers = db.PhoneUsers.Where(d => d.UserId == doc.Id).Select(d => d.PhoneNumber).ToList(),
                    Email = doc.Email,
                    SpecialistDoctors = db.DoctorSpecialists.Where(d => d.DoctorId == doc.Id).Select(d=>d.SpecialName).ToList(),
                    Gender = doc.Gender,
                    Age = doc.Age,
                    City = doc.City,
                    Country = doc.Country,
                    Region = doc.Region,
                    ImageName=doc.ImageName
                });

            }

            //return PartialView("_OurDoctors", doctors);
            return View(doctors);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}