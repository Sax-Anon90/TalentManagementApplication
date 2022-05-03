using AdminPortal.Common.Models;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface ICoursesRepository
    {
        Task<ICollection<CoursesVM>> GetAllCourses();
        Task<CoursesVM> GetCourseDetails(int? courseId);
        Task AddNewCourse(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM);
        Task UpdateCourse(CoursesVM courseUpdateModel);
        Task DeleteCourse(int Id);
    }
}
