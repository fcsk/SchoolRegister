using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolRegister.ViewModels.DTOs
{
    public class AddOrDeleteSubjectToGroupDto
    {
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int GroupId { get; set; }
    }
}
