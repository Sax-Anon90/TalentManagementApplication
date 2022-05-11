using AdminPortal.Common.Models.CoursesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.EmployeesViewModels
{
    public class EmployeeDetailsAndEmployeeFilesVM
    {
        public EmployeeVM EmployeeDetails { get; set; } = new EmployeeVM();
        public ICollection<EmployeeFileAttachmentsVM> EmployeeFileAttachmentsVM { get; set; } = new List<EmployeeFileAttachmentsVM>();
        public ICollection<CourseEnrollmentsVM> EmployeeCourseEnrollmentsVM { get; set; } = new List<CourseEnrollmentsVM>();
    }
}
