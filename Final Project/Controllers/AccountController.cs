using Final_Project.Models.DataContext;
using Final_Project.Models.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Final_Project.ViewModel;
using Final_Project.Repositary;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly UserRepositry userRepositry;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DataContext db;

        public AccountController(SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> roleManager,
            DataContext _db,UserManager<ApplicationUser> userManager,UserRepositry _userRepositry)
        {
           
            this.signInManager = _signInManager;
            this.roleManager = roleManager;
            db = _db;
            this.userManager = userManager;
            userRepositry = _userRepositry;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.GetUsersInRoleAsync("Doctor");
            List<UserRegisterVM> doctors = new List<UserRegisterVM>();
            foreach (var doc in users)
            {
                doctors.Add(new UserRegisterVM()
                {
                    Id=doc.Id,
                    UserName = doc.UserName,
                    //PhoneNumber = doc.PhoneNumber,
                    Email = doc.Email,
                    SpecialistDoctors = db.DoctorSpecialists.Where(d => d.DoctorId == doc.Id).Select(d => d.SpecialName).ToList(),
                    Gender = doc.Gender,
                    Age = doc.Age,
                    City = doc.City,
                    Country=doc.Country,
                    Region=doc.Region,
                   
                    ImageName = doc.ImageName
                    
                }) ; 
                
            }

            ViewBag.AllRoles = GetAllRoles();
            //return PartialView("_OurDoctors", doctors);
            return View("OurDoctors", doctors);
        }
        [HttpGet]

        public IActionResult Registration()
       {
            ViewBag.Clinics = GetAllClinics();
            ViewBag.AllRoles = GetAllRoles();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Registration(UserRegisterVM NewUser)
        {
            ViewBag.Clinics = GetAllClinics();
            ViewBag.AllRoles = GetAllRoles(); //view bag to retrun all Role 
          
                if (ModelState.IsValid)
                {
                    string imageName = userRepositry.UploadFile(NewUser.Image);
                    if (NewUser.RoleName == "Patient") { NewUser.ClinicId = null; }
                    if(NewUser.RoleName=="Doctor"&& NewUser.Region == null && NewUser.City == null)
                    {
                        ModelState.AddModelError("", "Region Or City Is Required");
                        return View(NewUser);

                    }

                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = NewUser.UserName,
                        Email = NewUser.Email,
                        Age = NewUser.Age,
                        Gender = NewUser.Gender,
                        Country = NewUser.Country,
                        City = NewUser.City,
                        Region = NewUser.Region,
                        ImageName = imageName,
                        ClinicId = NewUser.ClinicId,
                    };
                    // Create the new User record
                    IdentityResult result = await userManager.CreateAsync(user, NewUser.Password);
                    if (result.Succeeded)
                    {
                        ApplicationUser UserRegister = await userManager.FindByEmailAsync(user.Email);
                        // Get the last User ID
                        string userRegisterId = UserRegister.Id;
                        foreach (var phone in NewUser.PhoneNumbers)
                        {
                            db.PhoneUsers.Add(new PhoneUser()
                            {
                                UserId = userRegisterId,
                                PhoneNumber = phone
                            }
                         );
                        }
                        // Create a new phone User 

                        if (NewUser.RoleName == "Doctor")
                        {
                            foreach (var special in NewUser.SpecialistDoctors)
                            {
                                db.DoctorSpecialists.Add(new DoctorSpecialist
                                {
                                    DoctorId = userRegisterId,
                                    SpecialName = special
                                });

                            }

                        }
                        // Save the changes to the database
                        db.SaveChanges();

                        // Sign in the new  user
                        await signInManager.SignInAsync(user, false);

                        // Add the new  user to the role
                        await userManager.AddToRoleAsync(user, NewUser.RoleName);

                        // Redirect to the login page
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description); //return exeception if not success
                        }
                        return View(NewUser);
                    }
                }
                else
                {
                    return View(NewUser);
                }
        

           

        }

        private List<Clinic> GetAllClinics()
        {
            return db.Clinics.ToList();
        }

        private List<SelectListItem> GetAllRoles() //method return All Roles 
        {
            var AllRoles = roleManager.Roles.Where(d=>d.Name!="Admin").OrderByDescending(d=>d.Name).Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return AllRoles;
        }

        public async Task<bool> IsEmailAvailable(string Email)
        {
            // Check if the email address is already in use
            bool emailExists = await userManager.Users.AnyAsync(u => u.Email ==Email);
            if (emailExists == true)
                return false;
            return true;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = "/Home/Index")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM userLogin, string? ReturnUrl ="/Home/Index")
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = await userManager.FindByEmailAsync(userLogin.UserEmail); //Chech if user Exist or not
                if (User != null)
                {
                    //Create cookies
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync(User, userLogin.Password, userLogin.RemmberMe,false); //create cookie
                    if (result.Succeeded)
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                    else
                        ModelState.AddModelError("Password", "UserName or Password Not Correct");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password Not Correct");
                }
            }
            return View(userLogin);
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync(); //Killed Cookie and logout
            return RedirectToAction("Login");

        }

    }
}
