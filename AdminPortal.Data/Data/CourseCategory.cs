using System;
using System.Collections.Generic;

namespace AdminPortal.Data.Data
{
    public partial class CourseCategory
    {
        public CourseCategory()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
