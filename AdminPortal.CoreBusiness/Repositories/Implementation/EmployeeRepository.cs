using AdminPortal.Common.Models.EmployeesViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Data.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortal.Common.Models;

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

        public async Task UpdateEmployeeDetails(EmployeeVM employeeUpdateVM)
        {
            var employeeModel = _mapper.Map<Employee>(employeeUpdateVM);
            _dbContext.Employees.Update(employeeModel);
        }

        public async Task DeleteEmployee(string employeeNo)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeNo.Equals(employeeNo));
            _dbContext.Employees.Remove(employee);
        }

        public async Task<int> GetTotalNumberOfEmployees()
        {
            var totalEmployees = await _dbContext.Employees.CountAsync();
            return totalEmployees;
        }

        public async Task<ExcelFileDownloadProperties> GenerateExcelFileForTotalEmployees()
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExcelFileResult");
                int currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Employee No";
                worksheet.Cell(currentRow, 2).Value = "First Name";
                worksheet.Cell(currentRow, 3).Value = "Last Name";
                worksheet.Cell(currentRow, 4).Value = "Gender";
                worksheet.Cell(currentRow, 5).Value = "Department";
                worksheet.Cell(currentRow, 6).Value = "Position Title";

                var employeesData = await GetAllEmployees();
                foreach (var result in employeesData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = result.EmployeeNo;
                    worksheet.Cell(currentRow, 2).Value = result.FirstName;
                    worksheet.Cell(currentRow, 3).Value = result.LastName;
                    worksheet.Cell(currentRow, 4).Value = result.Gender;
                    worksheet.Cell(currentRow, 5).Value = result.Department;
                    worksheet.Cell(currentRow, 6).Value = result.PositionTitle;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var ExcelFile = new ExcelFileDownloadProperties()
                    {
                        FileName = "ExcelFileResult.xlsx",
                        FileType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet",
                        FileContent = content
                    };
                    return ExcelFile;
                }
            }
        }
    }
}
