using AdminPortal.Common.Models.EmployeesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IEmployeeFileAttachmentRepository
    {
        Task<bool> UploadEmployeeFileAttachment(EmployeeAndEmployeeTrainingVM employeeFileAttachmentModel, int employeeId);
        Task<ICollection<EmployeeFileAttachmentsVM>> GetAllEmployeeFileAttachmentsById(int employeeId);
        Task<EmployeeFileAttachmentsVM> GetEmployeeFileAttachment(int Id);
        Task DeleteEmployeeFileAttachment(int employeeFileAttachmentId);
        Task<int> GetTotalNumberOfEmployeeFiles(int employeeId);
        Task<int> GetTotalOfAllEmployeeFiles();
    }
}
