using AdminPortal.Common.Models.AuthenicationViewModels;
using AdminPortal.Common.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<UserViewModel> GetUserDetails(int userId);
        Task<ICollection<UserViewModel>> GetAllUsers();
        Task AddNewUser(UsersViewModel userCreateViewModel);
        Task UpdateUser(UserViewModel userViewModel);
        Task DeleteUser(int userId);
        Task<int> GetTotalNumberOfUsers();
        Task<bool> CheckIfUserExists(RegistrationViewModel registrationViewModel);
        Task<bool> SetUpPasswordForUser(SetUpPasswordViewModel setUpPasswordViewModel);
    }
}
