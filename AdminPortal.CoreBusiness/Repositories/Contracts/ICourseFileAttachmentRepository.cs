using AdminPortal.Common.Models;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface ICourseFileAttachmentRepository
    {
        Task<bool> UploadCourseFileAttachment(CoursesAndCourseCategoriesVM courseFileAttachment, int courseId);
        Task<ICollection<CourseFileAttachmentViewVM>> GetAllCourseFileAttachmentsById(int? CourseId);
        Task<CourseFileAttachmentViewVM> GetCourseFileAttachment(int Id);
        Task DeleteCourseFileAttachment(int courseFileAttachmentId);
    }
}
