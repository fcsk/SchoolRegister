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
        public virtual Group Group { get; set; }
        
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
