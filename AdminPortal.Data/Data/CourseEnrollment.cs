using System;
using System.Collections.Generic;

namespace AdminPortal.Data
{
    public partial class CourseEnrollment
    {
        public int Id { get; set; }
        public string? Status { get; set; }
        public int? EmployeeId { get; set; }
        public int? CourseId { get; set; }

        public virtual Course? Course { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
