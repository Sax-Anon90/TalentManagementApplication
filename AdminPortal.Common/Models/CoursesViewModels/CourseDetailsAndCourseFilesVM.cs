using AdminPortal.Common.Models.EmployeesViewModels;

namespace AdminPortal.Common.Models.CoursesViewModels
{
    public class CourseDetailsAndCourseFilesVM
    {
        public CoursesVM CourseDetails { get; set; } = new CoursesVM();
        public ICollection<CourseFileAttachmentViewVM> CourseFileAttachments { get; set; } = new List<CourseFileAttachmentViewVM>();
        public ICollection<CourseEnrollmentsVM> EmployeesInCourse { get; set; } = new List<CourseEnrollmentsVM>();
        //All employees who have completed the course
        public ICollection<CourseEnrollmentsVM> EmployeesCompletedCourses { get; set; } = new List<CourseEnrollmentsVM>();
        //Employees who are currently doing the course
        public ICollection<CourseEnrollmentsVM> EmployeesInProcessCourses { get; set; } = new List<CourseEnrollmentsVM>();
  
    }
}
