using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;

namespace SchoolRegister.Web.Controllers
{
    [Authorize]
    // [EnableCors("AllowCors")]
    [Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : Controller
    {
        protected readonly ILogger _logger;
        protected BaseController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BaseController>();
        }
    }
}