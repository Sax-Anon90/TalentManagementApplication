using AdminPortal.Data;
using AdminPortal.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AdminPortal.CoreBusiness.Repositories.Contracts;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
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
        public async Task<CoursesVM> GetCourseDetails(int? courseId)
        {
            if(courseId == null)
            {
                return null;
            }
            var course = await _dbContext.Courses
                .Include(x => x.CourseCategory)
                .ProjectTo<CoursesVM>(_configurationProvider)
                .FirstOrDefaultAsync(x => x.Id == courseId);
            return course;
        }
        public async Task<ICollection<CoursesVM>> GetAllCourses()
        {
            var courses = await _dbContext.Courses
                 .Include(x => x.CourseCategory)
                 .ProjectTo<CoursesVM>(_configurationProvider)
                 .ToListAsync();
            return courses;
        }
        public async Task AddNewCourse(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM)
        {
            CoursesVM courseCreateModel = new()
            {
                CourseName = coursesAndCourseCategoriesVM.CourseCreateViewModel.CourseName,
                CourseCategoryId = coursesAndCourseCategoriesVM.CourseCreateViewModel.CourseCategoryId
            };
            var course = _mapper.Map<Course>(courseCreateModel);
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCourse(CoursesVM courseUpdateModel)
        {
            CoursesVM courseEditModel = new()
            {
                Id = courseUpdateModel.Id,
                CourseName = courseUpdateModel.CourseName,
                CourseCategoryId = courseUpdateModel.CourseCategoryId
            };
            var courseModel = _mapper.Map<Course>(courseEditModel);
            _dbContext.Courses.Update(courseModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCourse(int Id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == Id);
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
