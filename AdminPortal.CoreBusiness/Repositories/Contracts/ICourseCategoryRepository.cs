
using AdminPortal.Common.Models;
using AdminPortal.Common.Models.CoursesViewModels;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface ICourseCategoryRepository
    {
        Task<ICollection<CourseCategoryVM>> GetAllCourseCategories();
        Task AddNewCourseCategory(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM);
        Task<CourseCategoryVM> GetCourseCategoryDetails(int courseCategoryId); 
        Task UpdateCourseCategory(CourseCategoryVM courseCategoryVM);
        Task DeleteCourseCategory(int courseCategoryId);
        Task<int> GetTotalNoOfCourseCategories();
        Task<int> GetTotalNumberOfCoursesInCourseCategory(int courseCategoryId);
        Task<ExcelFileDownloadProperties> GetAllCoursesInCourseCategoryToExcel(int courseCategoryId);
    }
}
