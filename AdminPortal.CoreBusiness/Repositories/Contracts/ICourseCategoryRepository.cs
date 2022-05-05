
using AdminPortal.Common.Models;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface ICourseCategoryRepository
    {
        Task<ICollection<CourseCategoryVM>> GetAllCourseCategories();
        Task AddNewCourseCategory(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM);
        Task<CourseCategoryVM> GetCourseCategoryDetails(int courseCategoryId); 
        Task UpdateCourseCategory(CourseCategoryVM courseCategoryVM);
        Task DeleteCourseCategory(int courseCategoryId);
    }
}
