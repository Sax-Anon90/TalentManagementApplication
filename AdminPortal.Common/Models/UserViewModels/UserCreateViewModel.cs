using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdminPortal.Common.Models.UserViewModels
{
    public class UserCreateViewModel
    {
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? Lastname { get; set; }
        [Display(Name = "Employee Number")]
        public string? EmployeeNo { get; set; }
        public string? Email { get; set; }

        public bool Validation()
        {
            if (FirstName is null || Lastname is null || EmployeeNo is null ||
                Email is null)
            {
                return false;
            }
            return true;
        }
    }

}
