using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using System.Text;
using System.Linq;

namespace SchoolRegister.Services.Services
{
    public class StudentService : BaseService, IStudentService
    {
        private IEnumerable<StudentVm> studentsEntities;

        public StudentService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public StudentVm GetStudent(Expression<Func<Student, bool>> filterPredicate)
        {
            if (filterPredicate == null)
            {
                throw new ArgumentNullException($"filterPredicate is null");
            }
            var studentEntity = _dbContext.Users.OfType<Student>().FirstOrDefault(filterPredicate);
            var studentVm = Mapper.Map<StudentVm>(studentEntity);
            return studentVm;
        }

        public IEnumerable<StudentVm> GetStudents(Expression<Func<Student, bool>> filterPredicate)
        {
            var studentsEntities = _dbContext.Users.OfType<Student>().AsQueryable();
            if (filterPredicate != null)
                studentsEntities = studentsEntities.Where(filterPredicate);
            var studentsVm = Mapper.Map<IEnumerable<StudentVm>>(studentsEntities);
            return studentsVm;
        }
    }
}
