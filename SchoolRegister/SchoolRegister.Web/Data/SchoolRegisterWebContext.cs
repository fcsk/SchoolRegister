using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.BLL.Entities;

namespace SchoolRegister.Web.Models
{
    public class SchoolRegisterWebContext : DbContext
    {
        public SchoolRegisterWebContext (DbContextOptions<SchoolRegisterWebContext> options)
            : base(options)
        {
        }

        public DbSet<SchoolRegister.BLL.Entities.Student> Student { get; set; }
    }
}
