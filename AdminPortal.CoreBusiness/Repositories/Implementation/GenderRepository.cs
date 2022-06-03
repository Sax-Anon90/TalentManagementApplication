using AdminPortal.Common.Models.CoursesViewModels;
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

    public class GenderRepository : IGenderRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public GenderRepository(SaxTalentManagementContext _dbContext, 
            IMapper _mapper, AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;   
            this._mapper = _mapper; 
            this._configurationProvider = _configurationProvider;

        }
        public async Task<ICollection<GenderVM>> GetAllGenders()
        {
            var genders = await _dbContext.Genders.AsNoTracking()
                .ProjectTo<GenderVM>(_configurationProvider)
                .ToListAsync();
            return genders;
        }
    }
}
