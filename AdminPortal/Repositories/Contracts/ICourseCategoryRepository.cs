using AdminPortal.Models;

namespace AdminPortal.Repositories.Contracts
{
    public interface ICourseCategoryRepository
    {
        Task<ICollection<CourseCategoryVM>> GetAllCourseCategories();
    }
}
