using AdminPortal.Common.Models.CoursesViewModels;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface ICourseFileAttachmentRepository
    {
        Task<bool> UploadCourseFileAttachment(CoursesAndCourseCategoriesVM courseFileAttachment, int courseId);
        Task<ICollection<CourseFileAttachmentViewVM>> GetAllCourseFileAttachmentsById(int? CourseId);
        Task<CourseFileAttachmentViewVM> GetCourseFileAttachment(int Id);
        Task DeleteCourseFileAttachment(int courseFileAttachmentId);
        Task<int> GetTotalNumberOfCourseFiles();
        Task<int> GetTotalNumberofCourseFilesById(int CourseId);
    }
}
