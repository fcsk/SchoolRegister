using Microsoft.AspNetCore.Identity;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace SchoolRegister.Services.Services
{
    public class GroupService : BaseService, IGroupService
    { 
        private UserManager<User> _userManager;
        public GroupService(ApplicationDbContext dbContext, UserManager<User> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public GroupVm AddOrUpdateGroup(AddOrUpdateGroupDto addOrUpdateGroupDto)
        {
            if (addOrUpdateGroupDto == null)
            {
                throw new ArgumentNullException($"Dto of type is null");
            }
            var groupEntity = Mapper.Map<Group>(addOrUpdateGroupDto);
            if (addOrUpdateGroupDto.Id == null || addOrUpdateGroupDto.Id ==0)
            {
                _dbContext.Groups.Add(groupEntity);
            }
            else
            {
                _dbContext.Groups.Update(groupEntity);
            }
            _dbContext.SaveChanges();
            var groupVm = Mapper.Map<GroupVm>(groupEntity);
            return groupVm;
        }

        public StudentVm AddStudentToGroup(AddOrDeleteStudentToGroupDto addStudentToGroupDto)
        {
            if ( addStudentToGroupDto == null)
            {
                throw new ArgumentNullException($"Dto type is null");
            }
            var student = _dbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == addStudentToGroupDto.StudentId);
            if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
            {
                throw new ArgumentNullException($"Student is null or user is not student");
            }

            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == addStudentToGroupDto.GroupId);
            if (group == null)
            {
                throw new ArgumentNullException($"group is null");
            }

            student.GroupId = group.Id;
            student.Group = group;
            _dbContext.SaveChanges();
            var studentVm = Mapper.Map<StudentVm>(student);
            return studentVm;
        }

        public GroupVm AddSubjectToGroup(AddOrDeleteSubjectToGroupDto addSubjectToGroupDto)
        {
            if (addSubjectToGroupDto == null)
            {
                throw new ArgumentNullException($"Dto of type is null");
            }
            var subjectGroup = _dbContext.SubjectGroup.FirstOrDefault(sg => sg.GroupId == addSubjectToGroupDto.GroupId && sg.SubjectId == addSubjectToGroupDto.SubjectId);
            if (subjectGroup != null)
            {
                throw new ArgumentNullException($"There is such attachment already defined.");
            }
            subjectGroup = new SubjectGroup
            {
                GroupId = addSubjectToGroupDto.GroupId,
                SubjectId = addSubjectToGroupDto.SubjectId
            };
            _dbContext.SubjectGroup.Add(subjectGroup);
            _dbContext.SaveChanges();
            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == addSubjectToGroupDto.GroupId);
            var groupVm = Mapper.Map<GroupVm>(group);
            return groupVm;
        }

        public StudentVm DeleteStudentToGroup(AddOrDeleteStudentToGroupDto deleteStudentToGroupDto)
        {
            if (deleteStudentToGroupDto == null)
            {
                throw new ArgumentNullException($"Dto of type is null");
            }
            var student = _dbContext.Users.OfType<Student>().FirstOrDefault(t => t.Id == deleteStudentToGroupDto.StudentId);
            if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
            {
                throw new ArgumentNullException($"Student is null or user is not student");
            }

            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == deleteStudentToGroupDto.GroupId);
            if (group == null)
            {
                throw new ArgumentNullException($"group is null");
            }

            student.GroupId = null;
            student.Group = null;
            group.Students.Remove(student);
            _dbContext.SaveChanges();
            var studentVm = Mapper.Map<StudentVm>(student);
            return studentVm;
        }

        public GroupVm DeleteSubjectToGroup(AddOrDeleteSubjectToGroupDto deleteSubjectToGroupDto)
        {
            if (deleteSubjectToGroupDto == null)
            {
                throw new ArgumentNullException($"Dto of type is null");
            }
            var subjectGroup = _dbContext.SubjectGroup.FirstOrDefault(sg => sg.GroupId == deleteSubjectToGroupDto.GroupId && sg.SubjectId == deleteSubjectToGroupDto.SubjectId);
            if (subjectGroup == null)
            {
                throw new ArgumentNullException($"The is no such attachment between group and subject");
            }
            _dbContext.SubjectGroup.Remove(subjectGroup);
            _dbContext.Remove(subjectGroup);
            _dbContext.SaveChanges();
            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == deleteSubjectToGroupDto.GroupId);
            var groupVm = Mapper.Map<GroupVm>(group);
            return groupVm;
        }

        public GroupVm GetGroup(Expression<Func<Group, bool>> filterPredicate)
        {
            if ( filterPredicate == null)
            {
                throw new ArgumentNullException($"Predicate is null");
            }
            var groupEntity = _dbContext.Groups.FirstOrDefault(filterPredicate);
            var groupVm = Mapper.Map<GroupVm>(groupEntity);

            return groupVm;
        }

        public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterPredicate = null)
        {
            var groupEntities = _dbContext.Groups
               .AsQueryable();
            if (filterPredicate != null)
            {
                groupEntities = groupEntities.Where(filterPredicate);
            }
            var groupVms = Mapper.Map<IEnumerable<GroupVm>>(groupEntities.ToList());
            return groupVms;
        }
    }
}
