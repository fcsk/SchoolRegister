using SchoolRegister.BLL.Entities;
using System;
using System.Linq.Expressions;


namespace SchoolRegister.ViewModels.DTOs
{
    public class GetTeacherGroups
    {
        public int TeacherId { get; set; }
        public Expression<Func<Student, bool>> GroupsFilterPredicate { get; set; }
    }
}
