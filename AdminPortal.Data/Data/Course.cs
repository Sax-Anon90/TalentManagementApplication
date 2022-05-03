using System;
using System.Collections.Generic;

namespace AdminPortal.Data
{
    public partial class Course
    {
        public Course()
        {
            CourseEnrollments = new HashSet<CourseEnrollment>();
            CourseFileAttachments = new HashSet<CourseFileAttachment>();
        }

        public int Id { get; set; }
        public string? CourseName { get; set; }
        public int? CourseCategoryId { get; set; }

        public virtual CourseCategory? CourseCategory { get; set; }
        public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; }
        public virtual ICollection<CourseFileAttachment> CourseFileAttachments { get; set; }
    }
}
