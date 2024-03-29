﻿
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AdminPortal.Data.Data;
using AdminPortal.Common.Models.CoursesViewModels;
using AdminPortal.Common.Models;
using AdminPortal.CoreBusiness.Services;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class CourseCategoryRepository : ICourseCategoryRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IExcelFileService _excelFileService;
        public CourseCategoryRepository(SaxTalentManagementContext _dbContext,
            IMapper _mapper, AutoMapper.IConfigurationProvider _configurationProvider,
            IExcelFileService _excelFileService)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._excelFileService = _excelFileService; 

        }

        public async Task<CourseCategoryVM> GetCourseCategoryDetails(int courseCategoryId)
        {
            var courseCategoryDetails = await _dbContext.CourseCategories
                .Include(x => x.Courses)
                .ProjectTo<CourseCategoryVM>(_configurationProvider)
                .FirstOrDefaultAsync(x => x.Id == courseCategoryId);
            return courseCategoryDetails;
        }

        public async Task AddNewCourseCategory(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM)
        {
            CourseCategoryVM courseCategoryCreateModel = new()
            {
                CategoryName = coursesAndCourseCategoriesVM.courseCategoryCreateVM.CategoryName
            };
            var courseCategory = _mapper.Map<CourseCategory>(courseCategoryCreateModel);
            await _dbContext.CourseCategories.AddAsync(courseCategory);
        }

        public async Task<ICollection<CourseCategoryVM>> GetAllCourseCategories()
        {
            var courseCategories = await _dbContext.CourseCategories
                 .Include(x => x.Courses)
                 .ProjectTo<CourseCategoryVM>(_configurationProvider)
                 .ToListAsync();
            return courseCategories;
        }

        public async Task UpdateCourseCategory(CourseCategoryVM courseCategoryVM)
        {
            CourseCategoryVM courseCategoryUpdateModel = new()
            {
                Id = courseCategoryVM.Id,
                CategoryName = courseCategoryVM.CategoryName
            };
            var courseCategory = _mapper.Map<CourseCategory>(courseCategoryUpdateModel);
            _dbContext.CourseCategories.Update(courseCategory);
        }

        public async Task DeleteCourseCategory(int courseCategoryId)
        {
            var courseCategory = await _dbContext.CourseCategories.FirstOrDefaultAsync(x => x.Id == courseCategoryId);
            _dbContext.CourseCategories.Remove(courseCategory); 
        }

        public async Task<int> GetTotalNoOfCourseCategories()
        {
            var totalCourseCategories = await _dbContext.CourseCategories.CountAsync();
            return totalCourseCategories;
        }

        public async Task<int> GetTotalNumberOfCoursesInCourseCategory(int courseCategoryId)
        {
           var totalCoursesInCategory = await _dbContext.Courses
                .Where(x => x.CourseCategoryId == courseCategoryId)
                .CountAsync();
            return totalCoursesInCategory;
        }

        public async Task<ExcelFileDownloadProperties> GetAllCoursesInCourseCategoryToExcel(int courseCategoryId)
        {
            var CoursesInCourseCategoryData = await _dbContext.Courses
                .Where(x => x.CourseCategoryId == courseCategoryId)
                .ProjectTo<CoursesVM>(_configurationProvider)
                .ToListAsync();
            var coursesInCourseCategoryExcelFile = await _excelFileService.GenerateExcelFileFromCoursesData(CoursesInCourseCategoryData);
            return coursesInCourseCategoryExcelFile;
            
        }
    }
}
