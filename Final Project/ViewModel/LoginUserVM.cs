using System.ComponentModel.DataAnnotations;

namespace Final_Project.ViewModel
{
    public class LoginUserVM
    {
        [Display(Name ="User Name"), Required(ErrorMessage ="Enter user name")]
        public string UserEmail { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Enter complex password")]
        public string Password { get; set; }

        public bool RemmberMe { get; set; }
    }
}
