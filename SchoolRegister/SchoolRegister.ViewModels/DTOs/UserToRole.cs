
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.ViewModels.DTOs
{
    public class UserToRoleDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}