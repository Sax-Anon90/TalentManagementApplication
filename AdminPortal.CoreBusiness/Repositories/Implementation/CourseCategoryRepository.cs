using AdminPortal.Data;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class CourseCategoryRepository : ICourseCategoryRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        public CourseCategoryRepository(SaxTalentManagementContext _dbContext,
            IMapper _mapper, AutoMapper.IConfigurationProvider _configurationProvider)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;

        }

        public async Task AddNewCourseCategory(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM)
        {
            CourseCategoryVM courseCategoryCreateModel = new()
            {
                CategoryName = coursesAndCourseCategoriesVM.courseCategoryCreateVM.CategoryName
            };
            var courseCategory = _mapper.Map<CourseCategory>(courseCategoryCreateModel);
            await _dbContext.CourseCategories.AddAsync(courseCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<CourseCategoryVM>> GetAllCourseCategories()
        {
            var courseCategories = await _dbContext.CourseCategories
                 .Include(x => x.Courses)
                 .ProjectTo<CourseCategoryVM>(_configurationProvider)
                 .ToListAsync();
            return courseCategories;
        }
    }
}
