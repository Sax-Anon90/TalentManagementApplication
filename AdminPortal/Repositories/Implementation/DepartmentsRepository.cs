using AdminPortal.Data;
using AdminPortal.Models;
using AdminPortal.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AdminPortal.Repositories.Implementation
{
    
    public class DepartmentsRepository : IDepartmentsRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public DepartmentsRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
           AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper; 
            this._configurationProvider = _configurationProvider;
        }
        public async Task<ICollection<DepartmentsVM>> GetAllDepartments()
        {
            var departments = await _dbContext.Departments
                .ProjectTo<DepartmentsVM>(_configurationProvider)
                .ToListAsync();
            return departments;
        }
    }
}
