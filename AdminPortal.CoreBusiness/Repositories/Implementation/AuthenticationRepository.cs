using AdminPortal.Common.Models.AuthenicationViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.CoreBusiness.Services;
using AdminPortal.Data.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IPasswordHashService _passwordHashService;
        public AuthenticationRepository(IPasswordHashService _passwordHashService,
            SaxTalentManagementContext _dbContext)
        {
            this._dbContext = _dbContext;
            this._passwordHashService = _passwordHashService;

        }
        public async Task<ClaimsPrincipal> Login(LoginViewModel loginViewModel)
        {
            var userExists = await CheckIfUserExistsForLogin(loginViewModel);
            if (userExists is null)
            {
                return null;
            }
            var userRoles = await GetUserRolesForLogin(userExists.Id);
            if (userRoles is null || userRoles.Count() == 0)
            {
                return null;
            }
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userExists.Email),
                new Claim(ClaimTypes.Name, userExists.FirstName)
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Rolename));
            }
            var user = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            claimsPrincipal.AddIdentity(user);
            return claimsPrincipal;
        }

        private async Task<User> CheckIfUserExistsForLogin(LoginViewModel loginViewModel)
        {
            var passwordHash = _passwordHashService.GeneratePasswordHash(loginViewModel.Password);
            var userExists = await _dbContext.Users.Where(x => x.Email.Trim() == loginViewModel.Email &&
            x.Password == passwordHash).FirstOrDefaultAsync();

            if (userExists == null)
            {
                return null;
            }
            return userExists;
        }
        private async Task<ICollection<UserRole>> GetUserRolesForLogin(int userId)
        {
            var userRoles = await _dbContext.UserRoles.Where(x => x.Userid == userId)
                .Include(x => x.Role)
                .Include(x => x.User)
                .ToListAsync();
            return userRoles;
        }
    }
}
