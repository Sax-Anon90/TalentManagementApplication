using AdminPortal.Common.Models;
using AdminPortal.Common.Models.EmployeesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IEmployeeRepository
    {
        Task<ICollection<EmployeeVM>> GetAllEmployees();
        Task AddNewEmployee (EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM);
        Task<EmployeeVM> GetEmployeeDetails(int? employeeId);

        Task<EmployeeSearchResultVM> SearchEmployeeByEmployeeNo(string employeeNo);
        Task UpdateEmployeeDetails(EmployeeVM employeeVM);
        Task DeleteEmployee(string employeeNo);
        Task<int> GetTotalNumberOfEmployees();

        Task<ExcelFileDownloadProperties> GenerateExcelFileForTotalEmployees();
    }
}
