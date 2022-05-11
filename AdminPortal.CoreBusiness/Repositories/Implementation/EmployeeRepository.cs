using AdminPortal.Common.Models.EmployeesViewModels;
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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public EmployeeRepository(SaxTalentManagementContext _dbContext, 
            IMapper _mapper, AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
        }

        public async Task AddNewEmployee(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM)
        {
            EmployeeCreateVM employeeCreateVM = employeeAndEmployeeTrainingVM.EmployeeCreateViewModel;
            var newEmployee = _mapper.Map<Employee>(employeeCreateVM);
            await _dbContext.Employees.AddAsync(newEmployee);
        }

        public async Task<ICollection<EmployeeVM>> GetAllEmployees()
        {
            var employees = await _dbContext.Employees
                 .ProjectTo<EmployeeVM>(_configurationProvider)
                 .ToListAsync();
            return employees;
        }

        public async Task<EmployeeVM> GetEmployeeDetails(int? employeeId)
        {
            var employeeDetails = await _dbContext.Employees
                .ProjectTo<EmployeeVM>(_configurationProvider)
                .FirstOrDefaultAsync(x => x.Id == employeeId);
            return employeeDetails;
        }

        public async Task<EmployeeSearchResultVM> SearchEmployeeByEmployeeNo(string employeeNo)
        {
           var searchResult = await _dbContext.Employees
                .ProjectTo<EmployeeSearchResultVM>(_configurationProvider)
                .FirstOrDefaultAsync(x => x.EmployeeNo.Equals(employeeNo));
            return searchResult;
        }
    }
}
