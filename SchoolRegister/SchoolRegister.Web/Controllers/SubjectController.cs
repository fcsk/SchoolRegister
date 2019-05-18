using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SchoolRegister.BLL.Entities;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using SchoolRegister.DAL.EF;

namespace SchoolRegister.Web.Controllers
{


    namespace SchoolRegister.Web.Controllers
    {
        [Authorize(Roles = "Admin")]
        public class SubjectController : BaseController
        {
            private readonly ISubjectService _subjectService;
            private readonly ITeacherService _teacherService;
            private readonly ApplicationDbContext _dbContext;
            private readonly UserManager<User> _userManager;
            public SubjectController(ISubjectService subjectService, ITeacherService teacherService, UserManager<User> userManager,
                 ILoggerFactory loggerFactory, ApplicationDbContext applicationDbContext) : base(loggerFactory)
            {
                _subjectService = subjectService;
                _dbContext = applicationDbContext;
                _userManager = userManager;
                _teacherService = teacherService;
            }

            public IActionResult GetSubjects()
            {
                var authenticatedUser = ((ClaimsIdentity)User?.Identity)?.Claims?.First()?.Value;
                if (authenticatedUser == null) return Unauthorized();
                var user = _dbContext.Users.FirstOrDefault(u => u.UserName == authenticatedUser);
                if (user == null) return StatusCode(500, "Given user does not exist in the database.");
                if (_userManager.IsInRoleAsync(user, "Admin").Result)
                {
                    return Ok(_subjectService.GetSubjects());
                }
                else
                {
                    if (_userManager.IsInRoleAsync(user, "Teacher").Result)
                    {
                        var teacher = _userManager.GetUserAsync(User).Result as Teacher;
                        return Ok(_subjectService.GetSubjects(x => x.TeacherId == teacher.Id));
                    }
                }
                return BadRequest("Error");
            }

            [HttpGet]
            public IActionResult GetTeachers()
            {
                var teachers = _teacherService.GetTeachers();
                return Ok(teachers.Select(t => new { id = t.Id, name = $"{t.Title} {t.FirstName} {t.LastName}" }));
            }

            [HttpPost]
            public IActionResult AddOrUpdateSubject([FromForm]AddOrUpdateSubjectDto subjectDto)
            {
                try
                {
                    if (ModelState == null || !ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    return Ok(_subjectService.AddOrUpdate(subjectDto));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    return StatusCode(500, "Error occured");
                }
            }

            [HttpDelete]
            public IActionResult DeleteSubject([FromForm]int id)
            {
                try
                {
                    if (_subjectService.DeleteSubject(s => s.Id == id))
                    {
                        return Ok(new { success = true });
                    }
                    return StatusCode(500, "Error occured");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    return StatusCode(500, "Error occured");
                }
            }

        }
    }
}