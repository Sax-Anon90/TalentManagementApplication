using AdminPortal.Common.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface IUserRolesRepository
    {
        Task<ICollection<UserRolesViewModel>> GetAllUserRoles(int? userId);
        Task<bool> AssignRolesToUser(int userId, UserRolesViewModel userRolesViewModel);
        Task<UserRolesViewModel> GetRolesNotAssignedToUser(int userId);
        Task DeleteUserRole(int userRoleId);
        Task<int> GetTotalNumberOfViewerRoles();
        Task<int> GetTotalNumberOfHRAdmins();
        Task<int> GetTotalNumberOfSuperAdmins();
        Task<int> GetTotalNumberOfUsersRoles(int userId);
    }
}
