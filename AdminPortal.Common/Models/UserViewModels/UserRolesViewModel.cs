using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.UserViewModels
{
    public class UserRolesViewModel : BaseViewModel.BaseViewModel
    {
        public int? Userid { get; set; }
        public int? Roleid { get; set; }
        public virtual RolesViewModel? Role { get; set; }
        public virtual UserViewModel? User { get; set; }

        //To only display the roles the user has not yet been assigned to 
        public IEnumerable<string> RolesNotAssignedToUser { get; set; } = new List<string>();

        //To populate all Roles in "RolesSelected" object and use it for checkbox selection
        public Dictionary<string, bool> RolesSelected { get; set; } = new Dictionary<string, bool>();
    }
}
