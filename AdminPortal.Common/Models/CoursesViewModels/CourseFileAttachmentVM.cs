using Microsoft.AspNetCore.Http;

namespace AdminPortal.Common.Models.CoursesViewModels
{
    public class CourseFileAttachmentVM
    {
        public IFormFile? courseFileAttachment { get; set; }
    }
}
