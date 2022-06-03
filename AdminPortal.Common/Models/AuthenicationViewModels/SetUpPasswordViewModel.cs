using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.AuthenicationViewModels
{
    public class SetUpPasswordViewModel
    {
        //These will be hidden 
        public string employeeNo { get; set; }
        public string Email { get; set; }

        [Display(Name = "Enter a new password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name = "Retype the password")]
        [DataType(DataType.Password)]
        public string passwordReEntered { get; set; }

        public bool Validation()
        {
            if (employeeNo is null || Email is null || password is null || passwordReEntered is null)
            {
                return false;
            }
            if(password != passwordReEntered)
            {
                return false;
            }
            return true;
        }
    }
}
