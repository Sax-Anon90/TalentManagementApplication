using AdminPortal.Data;
using AdminPortal.Models;

namespace AdminPortal.Repositories.Contracts
{
    public interface IDepartmentsRepository
    {
        Task<ICollection<DepartmentsVM>> GetAllDepartments();
    }
}
