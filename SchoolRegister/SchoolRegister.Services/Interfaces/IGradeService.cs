using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGradeService
    {
        GradeVm AddGradeToStudent(AddGradeToStudentDto addGradeToStudentDto);
        GradesRaportVm GetGradesRaportForStudents(GetGradesDto getGradesDto);
    }
}
