namespace AdminPortal.Common.Models
{
    public class CourseDetailsAndCourseFilesVM
    {
        public CoursesVM CourseDetails { get; set; } = new CoursesVM();
        public ICollection<CourseFileAttachmentViewVM> CourseFileAttachments { get; set; } = new List<CourseFileAttachmentViewVM>();

    }
}
