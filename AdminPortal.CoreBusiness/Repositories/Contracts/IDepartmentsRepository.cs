using AdminPortal.Common.Models.CoursesViewModels;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IDepartmentsRepository
    {
        Task<ICollection<DepartmentsVM>> GetAllDepartments();
    }
}
