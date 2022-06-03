using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.UserViewModels
{
    public class UsersViewModel
    {
        public ICollection<UserViewModel> AllUsersModel { get; set; } = new List<UserViewModel>();
        public UserCreateViewModel userCreateModel { get; set; } = new UserCreateViewModel();

    }
}
