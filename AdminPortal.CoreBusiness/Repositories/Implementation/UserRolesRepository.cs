using AdminPortal.Common.Models.UserViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Data.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IRolesRepository _rolesRepository;
        public UserRolesRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
             AutoMapper.IConfigurationProvider _configurationProvider,
              IRolesRepository _rolesRepository)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._rolesRepository = _rolesRepository;
        }
        public async Task<ICollection<UserRolesViewModel>> GetAllUserRoles(int? userId)
        {
            var userRoles = await _dbContext.UserRoles
                 .Include(x => x.User)
                 .Include(x => x.Role)
                 .Where(x => x.Userid == userId)
                 .ProjectTo<UserRolesViewModel>(_configurationProvider)
                 .ToListAsync();
            return userRoles;
        }
        public async Task<bool> AssignRolesToUser(int userId, UserRolesViewModel userRolesViewModel)
        {
            //Get all the roles the user selected from dictionary object
            var rolesSelected = userRolesViewModel.RolesSelected.Where(x => x.Value is true).Select(x => x.Key);
            if(rolesSelected.Count() is 0)
            {
                return false;
            }
            /*
              From the roles the user selected, get all the roles from the roles table
              and only get the ones the user selected so that we can get the Ids of those roles
             */
            var allRoles = await _rolesRepository.GetAllRoles();
            var rolesToGetRoleIds = allRoles.Where(x => rolesSelected.Any(y => x.RoleName.Contains(y)));

            var rolesToAssignToUser = new List<UserRolesViewModel>();
            foreach(var role in rolesToGetRoleIds)
            {
                rolesToAssignToUser.Add(new UserRolesViewModel()
                {
                    Userid = userId,
                    Roleid = role.Id,
                });
            }
            var userRoles = _mapper.Map<ICollection<UserRole>>(rolesToAssignToUser);
            await _dbContext.UserRoles.AddRangeAsync(userRoles);
            return true;
        }
        public async Task<UserRolesViewModel> GetRolesNotAssignedToUser(int userId)
        {
            var allRoles = await _rolesRepository.GetAllRoles();
            var userRoles = await GetAllUserRoles(userId);

            /*
             From all the Roles in the Roles table, select only the one's the user has not yet been assigned to
            */
            var rolesNotAssignedToUser = allRoles.Select(x => x.RoleName)
                .Where(x => !userRoles.Select(x => x.Role.RoleName).Any(y => x.Contains(y)));

            //This means the user has been assigned all the roles
            if(rolesNotAssignedToUser.Count() is 0 || rolesNotAssignedToUser is null)
            {
                return null;
            }

            var rolesNotAssigned = new UserRolesViewModel
            {
                RolesNotAssignedToUser = rolesNotAssignedToUser
            };

            foreach(var role in rolesNotAssignedToUser)
            {
                rolesNotAssigned.RolesSelected.Add(role, false);
            }
            return rolesNotAssigned;

        }

        public async Task DeleteUserRole(int userRoleId)
        {
            var userRole = await _dbContext.UserRoles.Where(x => x.Id == userRoleId)
                //Just to observe if we are deleting the right user role for debugging
                .Include(x => x.Role)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
            _dbContext.UserRoles.Remove(userRole);
        }

        public async Task<int> GetTotalNumberOfViewerRoles()
        {
            int totalViewers = await _dbContext.UserRoles
                 .Where(x => x.Role.Rolename.Equals("Viewer"))
                 .CountAsync();
            return totalViewers;
        }
        public async Task<int> GetTotalNumberOfHRAdmins()
        {
            int totalViewers = await _dbContext.UserRoles
                  .Where(x => x.Role.Rolename.Equals("HR Administrator"))
                  .CountAsync();
            return totalViewers;
        }
        public async Task<int> GetTotalNumberOfSuperAdmins()
        {
            int totalViewers = await _dbContext.UserRoles
                  .Where(x => x.Role.Rolename.Equals("Super Administrator"))
                  .CountAsync();
            return totalViewers;
        }

        public async Task<int> GetTotalNumberOfUsersRoles(int userId)
        {
            int totalUsersRoles = await _dbContext.UserRoles
                 .Where(x => x.Userid == userId).CountAsync();
            return totalUsersRoles;
                
        }
    }
}
