using SchoolRegister.BLL.Entities;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SchoolRegister.Services.Interfaces
{
    public interface IGroupService
    {
        GroupVm AddOrUpdateGroup(AddOrUpdateGroupDto addOrUpdateGroupDto);
        GroupVm GetGroup(Expression<Func<Group, bool>> filterPredicate);
        GroupVm AddSubjectToGroup(AddOrDeleteSubjectToGroupDto addSubjectToGroupDto);
        GroupVm DeleteSubjectToGroup(AddOrDeleteSubjectToGroupDto deleteSubjectToGroupDto);
        StudentVm AddStudentToGroup(AddOrDeleteStudentToGroupDto addStudentToGroupDto);
        StudentVm DeleteStudentToGroup(AddOrDeleteStudentToGroupDto deleteStudentToGroupDto);
        
    }
}
