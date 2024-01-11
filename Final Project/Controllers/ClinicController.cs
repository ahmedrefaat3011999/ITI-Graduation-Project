using Final_Project.Models.DataContext;
using Final_Project.Models.DomainModels;
using Final_Project.Repositary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Final_Project.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ClinicController : Controller
    {
        public DataContext db { get; set; }
        IRepository<Clinic> repostory;
        //private readonly UserManager<IdentityUser> userManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        //private IProduct Product;
        public ClinicController(IRepository<Clinic> _repostory, DataContext _db, RoleManager<IdentityRole> _roleManager)
        {
            repostory = _repostory;
            db = _db;
            roleManager = _roleManager;
        }


        public async Task<IActionResult> Index()
        {
            //var clinics = db.Clinics.Include("Appointments").Include("Doctors").ToList();
            var clinics = db.Clinics.ToList();
            //.Include(clinic => clinic.Appointments)
            //.Include(clinic => clinic.Clinic_Doctors)
            //.ToList();
            ////////////////////

            //var AllDoctortoClinic=db.Doctors.Where(d=>d.ClinicId== "8093f94c-331f-439e-84b0-adac6d760dcc").ToList();

            //var users = await userManager.GetUsersInRoleAsync("Doctor");
            //ViewBag.AllDoctors = users;
            //ViewBag.AllClinics = db.Clinics.ToList();
            //ViewBag.AllSpecialist = db.DoctorSpecialists.ToList();




            // Pass it to ViewBag
            //  ViewBag.DoctorNames = applicationUsers;



            return View(clinics);
        }


        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {


            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Clinic NewClinic)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    //if(userId == null)
                    //{
                    var clinicValidate = db.Clinics.Where(C => C.Name == NewClinic.Name || C.Email == NewClinic.Email).FirstOrDefault();
                    if (clinicValidate != null)
                    {

                        ModelState.AddModelError("", "Clinic Name or Email is alReady Exists before ");
                        return View();

                    }
                    repostory.Add(NewClinic);
                    repostory.Save();
                    //return LocalRedirect("/Home/Index");
                    TempData["NewDeptID"] = NewClinic.Name;
                    //ViewBag.CreatedClinic= NewClinic;
                    return View();

                    //}


                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Clinic is invalid " + ex.Message);
                }
            }


            return View(NewClinic);
        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View(repostory.GetByID(id));
        }


        [HttpPost]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Edit(string id, Clinic ClinicNew)
        {

            Clinic ClinicEdited = repostory.GetByID(id);
            if (ModelState.IsValid)
            {

                //var userID = ProductEdited.UserID;
                if (ClinicEdited != null)
                {
                    ClinicEdited.Name = ClinicNew.Name;
                    ClinicEdited.Email = ClinicNew.Email;
                    ClinicEdited.Country = ClinicNew.Country;
                    ClinicEdited.City = ClinicNew.City;
                    ClinicEdited.Region = ClinicNew.Region;
                    ClinicEdited.StartDate= ClinicNew.StartDate;
                    ClinicEdited.EndDate= ClinicNew.EndDate;    
                    ClinicEdited.PhoneClinic = ClinicNew.PhoneClinic;   
                }
                //repostory.Update(ProductEdited);
                repostory.Save();
                TempData["NewDeptID"] = ClinicNew.Name;
                return View();
            }

            return View(repostory.GetByID(id));
        }

        public IActionResult Delete(string id)
        {
            repostory.Delete(id);
            repostory.Save();
            return RedirectToAction("Index");

        }


        public IActionResult Details(string id)
        {
            if (repostory.GetByID(id) == null)
            {
                return NotFound();
            }
            return View(repostory.GetByID(id));
        }


        


    }
}










