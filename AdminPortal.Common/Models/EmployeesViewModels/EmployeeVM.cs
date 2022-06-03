using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.EmployeesViewModels
{
    public class EmployeeVM : BaseViewModel.BaseViewModel
    {
        [Display(Name="Employee Number")]
        public string? EmployeeNo { get; set; }
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Department { get; set; }
        public string? Gender { get; set; }
        [Display(Name = "Position Title")]
        public string? PositionTitle { get; set; }

        public bool Valdiation()
        {
            if(Id is 0 || EmployeeNo is null || FirstName is null || LastName is null ||
                Department is null || Gender is null || PositionTitle is null)
            {
                return false;
            }
            return true;
        }
    }
}
