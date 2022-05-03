using Microsoft.AspNetCore.Http;

namespace AdminPortal.Common.Models
{
    public class CourseFileAttachmentVM
    {
        public IFormFile? courseFileAttachment { get; set; }
    }
}
