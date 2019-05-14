using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolRegister.BLL.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        [ForeignKey("Teacher")]
        public int TeacherID { get; set; }
        public string Description { get; set; }
        public IList<SubjectGroup> SubjectGroups { get; set; }
        public IList<Grade> Grades { get; set; }
        
    }
}
