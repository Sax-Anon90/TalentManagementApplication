using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.AuthenicationViewModels
{
    public class RegistrationViewModel
    {
        [Display(Name = "Employee No")]
        public string employeeNo { get; set; }
        public string Email { get; set; }

        public bool Validation()
        {
            if(employeeNo is null || Email is null)
            {
                return false;
            }
            return true;
        }
    }
}
