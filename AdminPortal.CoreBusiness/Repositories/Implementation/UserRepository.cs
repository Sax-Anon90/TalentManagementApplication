using AdminPortal.Common.Models.AuthenicationViewModels;
using AdminPortal.Common.Models.UserViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.CoreBusiness.Services;
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
    public class UserRepository : IUserRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IPasswordHashService _passwordHashService;
        public UserRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider,
            IPasswordHashService _passwordHashService)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._passwordHashService = _passwordHashService;
        }

        public async Task<UserViewModel> GetUserDetails(int userId)
        {
            var user = await _dbContext.Users
                .ProjectTo<UserViewModel>(_configurationProvider)
                .FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }
        public async Task AddNewUser(UsersViewModel userCreateViewModel)
        {
            UserCreateViewModel userCreateModel = new()
            {
                FirstName = userCreateViewModel.userCreateModel.FirstName,
                Lastname = userCreateViewModel.userCreateModel.Lastname,
                EmployeeNo = userCreateViewModel.userCreateModel.EmployeeNo,
                Email = userCreateViewModel.userCreateModel.Email
            };
            var userModel = _mapper.Map<User>(userCreateModel);
            await _dbContext.Users.AddAsync(userModel);
        }

        public async Task<ICollection<UserViewModel>> GetAllUsers()
        {
            var users = await _dbContext.Users
                .ProjectTo<UserViewModel>(_configurationProvider)
                .ToListAsync();
            return users;
        }

        public async Task UpdateUser(UserViewModel userViewModel)
        {
            var user = await GetUserDetails(userViewModel.Id);

            var userUpdateModel = new UserViewModel()
            {
                Id = userViewModel.Id,
                EmployeeNo = userViewModel.EmployeeNo,
                FirstName = userViewModel.FirstName,
                Lastname = userViewModel.Lastname,
                Email = userViewModel.Email,
                Password = user.Password
            };

            var userModel = _mapper.Map<User>(userUpdateModel);
            _dbContext.Users.Update(userModel);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            _dbContext.Users.Remove(user);
        }

        public async Task<int> GetTotalNumberOfUsers()
        {
            int totalUsers = await _dbContext.Users.CountAsync();
            return totalUsers;
        }

        public async Task<bool> CheckIfUserExists(RegistrationViewModel registrationViewModel)
        {
            var user = await _dbContext.Users
                .Where(x => x.EmployeeNo == registrationViewModel.employeeNo && x.Email == registrationViewModel.Email
                && x.Password == null)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<bool> SetUpPasswordForUser(SetUpPasswordViewModel setUpPasswordViewModel)
        {
            var user = await _dbContext.Users.Where(x => x.Email == setUpPasswordViewModel.Email
            && x.EmployeeNo == setUpPasswordViewModel.employeeNo).FirstOrDefaultAsync();
            if (user is null)
            {
                return false;
            }
            var passwordHash = _passwordHashService.GeneratePasswordHash(setUpPasswordViewModel.password);
            user.Password = passwordHash;
            _dbContext.Users.Update(user);
            return true;
        }
    }
}
