using AdminPortal.Common.Models;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IDepartmentsRepository
    {
        Task<ICollection<DepartmentsVM>> GetAllDepartments();
    }
}
