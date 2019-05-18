using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using SchoolRegister.Web.Extensions;

namespace SchoolRegister.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtIssuerOptions> jwtOptions,
            ILoggerFactory loggerFactory,
            ApplicationDbContext dbContext
        )
            : base(loggerFactory)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        // GET: api/values
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<User>(dto);
                    var result = await _userManager.CreateAsync(entity, dto.Password);
                    if (result.Succeeded)
                    {
                        return Ok(new { success = true });
                    }

                    return StatusCode(500, result.Errors.GetErrorsString());
                }

                return StatusCode(500, "Invalid data model.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " Inner Message " + ex.InnerException?.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddUserToRole([FromForm]UserToRoleDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(dto.UserName);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, dto.Role);
                        if (result.Succeeded)
                        {
                            return Ok(new { success = true });
                        }

                        return StatusCode(500, result.Errors.GetErrorsString());
                    }

                    return StatusCode(500, "User not exists.");
                }

                return StatusCode(500, "Invalid data model.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " Inner Message " + ex.InnerException?.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> DetachUserFromRole([FromForm]UserToRoleDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(dto.UserName);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, dto.Role);
                        if (result.Succeeded)
                        {
                            return Ok(new { success = true });
                        }

                        return StatusCode(500, result.Errors.GetErrorsString());
                    }

                    return StatusCode(500, "User not exists.");
                }

                return StatusCode(500, "Invalid data model.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " Inner Message " + ex.InnerException?.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromForm] LoginUserVm applicationUser)
        {
            if (string.IsNullOrWhiteSpace(applicationUser.Login) || string.IsNullOrWhiteSpace(applicationUser.Password) || !ModelState.IsValid)
            {
                _logger.LogInformation(
                    $"Login or password are empty or invalid.");
                return BadRequest("Invalid credentials");
            }
            var result = _signInManager
                .PasswordSignInAsync(applicationUser.Login, applicationUser.Password, true, false).Result;
            if (result.Succeeded == false)
            {
                _logger.LogInformation(
                    $"Invalid username ({applicationUser.Login}) or password ({applicationUser.Password})");
                return BadRequest("Invalid credentials");
            }

            int tokenExpirationMinutes = 20;
            var claims = new[]
            {
                new Claim("username", applicationUser.Login),
                new Claim(JwtRegisteredClaimNames.Nbf,
                    new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,
                    ((long)((DateTime.Now.AddMinutes(tokenExpirationMinutes) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds)).ToString())
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = token.ValidTo.ToString("yyyy-MM-ddTHH:mm:ss")
            };
            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var authenticatedUser = ((ClaimsIdentity)User?.Identity)?.Claims?.First()?.Value;
            if (authenticatedUser == null) return Unauthorized();
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == authenticatedUser);
            if (user == null) return StatusCode(500, "Given user does not exist in the database.");
            var userIndividualVm = Mapper.Map<BaseUserVm>(user);
            return Ok(userIndividualVm);
        }
    }
}