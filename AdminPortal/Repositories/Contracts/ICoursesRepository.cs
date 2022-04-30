using AdminPortal.Models;

namespace AdminPortal.Repositories.Contracts
{
    public interface ICoursesRepository
    {
        Task<ICollection<CoursesVM>> GetAllCourses();
    }
}
