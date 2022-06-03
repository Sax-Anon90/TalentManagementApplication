namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseCategoryRepository CourseCategoriesRepository { get; }
        ICoursesRepository CoursesRepository { get; }
        ICourseFileAttachmentRepository CourseFileAttachmentsRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDepartmentsRepository DepartmentsRepository { get; }
        IGenderRepository GenderRepository { get; }
        IEmployeeFileAttachmentRepository EmployeeFileAttachmentsRepository { get; }
        ICourseEnrollmentsRepository CoursesEnrollmentsRepository { get; }
        IUserRepository UserRepository { get; }
        IRolesRepository RoleRepository { get; }
        IUserRolesRepository UserRolesRepository { get; }
        IAuthenticationRepository AuthenticationRepository { get; }
        Task SaveChangesAsync();
    }
}
