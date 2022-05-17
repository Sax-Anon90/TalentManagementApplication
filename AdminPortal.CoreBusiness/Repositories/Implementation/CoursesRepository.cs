using AdminPortal.Data.Data;
using AdminPortal.Common.Models.CoursesViewModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Common.Models;
using AdminPortal.CoreBusiness.Services;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IExcelFileService _excelFileService;
        public CoursesRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider, IExcelFileService _excelFileService)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._excelFileService = _excelFileService;
        }
        public async Task<CoursesVM> GetCourseDetails(int? courseId)
        {
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
        }

        public async Task UpdateCourse(CoursesVM courseUpdateModel)
        {
            var courseModel = _mapper.Map<Course>(courseUpdateModel);
            _dbContext.Courses.Update(courseModel);
        }

        public async Task DeleteCourse(int Id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == Id);
            _dbContext.Courses.Remove(course);
        }

        public async Task<int> GetTotalNumberOfCourses()
        {
            var totalCourses = await _dbContext.Courses.CountAsync();
            return totalCourses;
        }

        public async Task<ExcelFileDownloadProperties> GetAllCoursesDataToExcelFile()
        {
            var coursesData = await GetAllCourses();
            var courseExcelFile = await _excelFileService.GenerateExcelFileFromCoursesData(coursesData);
            return courseExcelFile;
        }
    }
}
