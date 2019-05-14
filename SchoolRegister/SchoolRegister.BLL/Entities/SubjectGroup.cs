using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolRegister.BLL.Entities
{
    public class SubjectGroup
    {
        [Key]
        public int GroupId { get; set; }
        [Key]
        public int Subject { get; set; }
    }
}
