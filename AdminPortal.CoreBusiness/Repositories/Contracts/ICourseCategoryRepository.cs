
using AdminPortal.Common.Models;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface ICourseCategoryRepository
    {
        Task<ICollection<CourseCategoryVM>> GetAllCourseCategories();
        Task AddNewCourseCategory(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM);
    }
}
