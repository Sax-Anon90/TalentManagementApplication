using AdminPortal.CoreBusiness.Repositories.Contracts;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        ICourseCategoryRepository CourseCategoriesRepository { get; }
        ICoursesRepository CoursesRepository { get; }
        ICourseFileAttachmentRepository CourseFileAttachmentsRepository { get; }

    }
}
