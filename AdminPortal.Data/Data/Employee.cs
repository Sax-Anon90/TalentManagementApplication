using System;
using System.Collections.Generic;

namespace AdminPortal.Data
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
        public string? Title { get; set; }
        public string? Department { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string? Race { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Region { get; set; }
        public string? ReportsToManager { get; set; }
        public string? PositionTitle { get; set; }
        public string? University { get; set; }
        public string? HighestQualification { get; set; }

        public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; }
        public virtual ICollection<EmployeeFileAttachment> EmployeeFileAttachments { get; set; }
    }
}
