using SchoolRegister.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolRegister.ViewModels.VMs
{
    public class GradeVm
    {
        public string SubjectName { get; set; }
        public GradeScale GradeValue { get; set; }
    }
}
