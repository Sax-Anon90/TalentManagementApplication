using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.EmployeesViewModels
{
    public class EmployeeAndEmployeeTrainingVM
    {
        public ICollection<EmployeeVM> EmployeesViewModel { get; set; } = new List<EmployeeVM>();
        public EmployeeCreateVM EmployeeCreateViewModel { get; set; } = new EmployeeCreateVM();
        public EmployeeFileAttachmentVM EmployeeFileAttachment { get; set; } = new EmployeeFileAttachmentVM();
        public EmployeeSearchResultVM EmployeeSearchResult { get; set; }
    }
}
