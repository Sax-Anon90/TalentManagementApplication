using Microsoft.AspNetCore.Http;

namespace AdminPortal.Common.Models.CoursesViewModels
{
    public class CourseFileAttachmentVM
    {
        public int employeeId { get; set; }
        public IFormFile? courseFileAttachment { get; set; }
    }
}
