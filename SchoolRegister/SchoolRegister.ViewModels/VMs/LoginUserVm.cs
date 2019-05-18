using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.VMs
{
    public class LoginUserVm
    {
        [Required(ErrorMessage = "Podaj login")]
        [Display(Name = "Login")]
        [StringLength(100, ErrorMessage = " {0} nie może przekraczac {1} znaków")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Podaj hasło")]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = " {0} nie może przekraczac {1} znaków")]
        public string Password { get; set; }
    }
}