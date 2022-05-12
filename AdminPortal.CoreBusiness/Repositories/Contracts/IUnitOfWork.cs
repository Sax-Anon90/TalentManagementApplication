namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        ICourseCategoryRepository CourseCategoriesRepository { get; }
        ICoursesRepository CoursesRepository { get; }
        ICourseFileAttachmentRepository CourseFileAttachmentsRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentsRepository DepartmentsRepository { get; }
        IGenderRepository GenderRepository { get; }
        IEmployeeFileAttachmentRepository EmployeeFileAttachmentsRepository { get; }
        ICourseEnrollmentsRepository CoursesEnrollmentsRepository { get; }
        Task SaveChangesAsync();
    }
}
