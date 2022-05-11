using System;
using System.Collections.Generic;

namespace AdminPortal.Data.Data
{
    public partial class Employee
    {
        public Employee()
        {
            CourseEnrollments = new HashSet<CourseEnrollment>();
            EmployeeFileAttachments = new HashSet<EmployeeFileAttachment>();
        }

        public int Id { get; set; }
        public string? EmployeeNo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Department { get; set; }
        public string? Gender { get; set; }
        public string? PositionTitle { get; set; }

        public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; }
        public virtual ICollection<EmployeeFileAttachment> EmployeeFileAttachments { get; set; }
    }
}
