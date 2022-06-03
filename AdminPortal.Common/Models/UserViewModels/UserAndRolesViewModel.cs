using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.UserViewModels
{
    public class UserAndRolesViewModel
    {
        public UserViewModel user { get; set; } = new UserViewModel();
        public ICollection<UserRolesViewModel> userRoles { get; set; } = new List<UserRolesViewModel>();
    }
}
