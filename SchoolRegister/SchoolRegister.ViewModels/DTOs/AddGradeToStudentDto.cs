using SchoolRegister.BLL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolRegister.ViewModels.DTOs
{
    public class AddGradeToStudentDto
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required] 
        public int TeacherId { get; set; }
        [Required]
        public GradeScale GradeValue { get; set; }
    }
}
