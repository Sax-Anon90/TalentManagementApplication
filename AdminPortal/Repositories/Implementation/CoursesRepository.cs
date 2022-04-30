using AdminPortal.Models;
using AdminPortal.Data;
using AdminPortal.Repositories.Contracts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Repositories.Implementation
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public CoursesRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
        }
        public async Task<ICollection<CoursesVM>> GetAllCourses()
        {
           var courses = await _dbContext.Courses
                .Include(x => x.CourseCategory)
                .ProjectTo<CoursesVM>(_configurationProvider)
                .ToListAsync();
            return courses;
        }
    }
}
