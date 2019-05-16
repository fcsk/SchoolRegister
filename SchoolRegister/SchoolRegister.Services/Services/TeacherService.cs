using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;

namespace SchoolRegister.Services.Services
{
    public class TeacherService : BaseService, ITeacherService
    {
        private readonly SmtpClient _smtpClient;
        private readonly UserManager<User> _userManager;
        private readonly IGroupService _groupService;
        public TeacherService(SmtpClient smtpClient, ApplicationDbContext dbContext, UserManager<User> userManager, IGroupService groupService) : base(dbContext)
        {
            _smtpClient = smtpClient;
            _userManager = userManager;
            _groupService = groupService;
        }

        public TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate)
        {
            var teacherEntity = _dbContext.Users.OfType<Teacher>().FirstOrDefault();
            if (teacherEntity == null)
            {
                throw new InvalidOperationException("There is no such teacher");
            }

            var teacherVm = Mapper.Map<TeacherVm>(teacherEntity);
            return teacherVm;
        }

        public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null)
        {
            var teacherEntities = _dbContext.Users.OfType<Teacher>()
                                    .AsQueryable();
            if (filterPredicate != null)
            {
                teacherEntities = teacherEntities.Where(filterPredicate);
            }
            var teacherVms = Mapper.Map<IEnumerable<TeacherVm>>(teacherEntities);
            return teacherVms;
        }

        public IEnumerable<GroupVm> GetTeachersGroups(GetTeacherGroups getTeachersGroupsDto)
        {
            if (getTeachersGroupsDto == null)
            {
                throw new ArgumentNullException($"Dto is null");
            }
            var teacher = _userManager.Users.OfType<Teacher>().FirstOrDefault(x => x.Id == getTeachersGroupsDto.TeacherId);
            var teacherGroups = _groupService.GetGroups(g => teacher.Subjects.SelectMany(s => s.SubjectGroups.Select(gr => gr.Group)).Any(x => x.Id == g.Id));
            return teacherGroups;
        }

        public bool SendEmailToParent(SendMailToParentsDto sendMailToParentsDto)
        {
            try
            {
                if (sendMailToParentsDto == null)
                {
                    throw new ArgumentNullException($"Dto is null");
                }

                var teacher = _dbContext.Users.OfType<Teacher>()
                    .FirstOrDefault(x => x.Id == sendMailToParentsDto.SenderId);
                if (teacher == null || _userManager.IsInRoleAsync(teacher, "Teacher").Result == false)
                {
                    throw new InvalidOperationException("sender is not teacher");
                }

                var student = _dbContext.Users.OfType<Student>().FirstOrDefault(x => x.Id == sendMailToParentsDto.StudentId);
                if (student == null || !_userManager.IsInRoleAsync(student, "Student").Result)
                {
                    throw new InvalidOperationException("given user is not student");
                }

                var mailMessage = new MailMessage(to: student.Parent.Email,
                    subject: sendMailToParentsDto.Title,
                    body: sendMailToParentsDto.Content,
                    from: teacher.Email);
                _smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            _smtpClient.Dispose();
            base.Dispose(disposing);
        }


    }
}
