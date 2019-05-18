using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Podaj login")]
        [Display(Name = "Login")]
        [StringLength(100, ErrorMessage = " {0} nie może przekraczac {1} znaków")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = " {0} nie może przekraczac {1} znaków")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Podaj hasło")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = " {0} musi mieć przynajmniej {2} znaków", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potwierdź hasło")]
        [Display(Name = "Potwierdź hasło")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = " {0} musi mieć przynajmniej {2} znaków", MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
}