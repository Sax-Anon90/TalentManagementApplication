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
    public class RolesRepository : IRolesRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public RolesRepository(SaxTalentManagementContext _dbContext, IMapper mapper,
            AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;
            this._mapper = mapper;  
            this._configurationProvider = _configurationProvider;
        }
        public async Task<ICollection<RolesViewModel>> GetAllRoles()
        {
            var allRoles = await _dbContext.Roles
                 .ProjectTo<RolesViewModel>(_configurationProvider)
                 .ToListAsync();
            return allRoles;
        }
    }
}
