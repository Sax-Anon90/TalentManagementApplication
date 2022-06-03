using AdminPortal.Common.Models;
using AdminPortal.Common.Models.EmployeesViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.CoreBusiness.Services;
using AdminPortal.Data.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Implementation
{
    public class CourseEnrollmentsRepository : ICourseEnrollmentsRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IExcelFileService _excelFileService;
        public CourseEnrollmentsRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider, ICoursesRepository _coursesRepository,
            IExcelFileService _excelFileService)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._coursesRepository = _coursesRepository;
            this._excelFileService = _excelFileService;
        }
        public async Task<CourseEnrollmentsVM> GetEmployeeCourseEnrollment(int employeeCourseEnrollmentId)
        {
            var employeeCourseEnrollment = await _dbContext.CourseEnrollments
                .Include(x => x.Course)
                .ProjectTo<CourseEnrollmentsVM>(_configurationProvider)
                .FirstOrDefaultAsync(x => x.Id == employeeCourseEnrollmentId);
            return employeeCourseEnrollment;
        }

        public async Task<CourseEnrollmentsVM> GetCoursesNotEnrolledByEmployee(int? employeeId)
        {
            var allCourses = await _coursesRepository.GetAllCourses();
            var courseEnrollments = await GetAllEmployeeCourseEnrollments(employeeId);

            /*
              From all the courses in the courses table, select only the one's the employee has not yet enrolled in
             */
            var coursesNotEnrolledByEmployee = allCourses.Select(x => x.CourseName)
                .Where(x => !courseEnrollments.Select(x => x.Course.CourseName).Any(y => x.Contains(y)));

            //This means the employee has enrolled in all courses
            if (coursesNotEnrolledByEmployee.Count() is 0 || coursesNotEnrolledByEmployee is null)
            {
                return null;
            }

            var coursesNotEnrolled = new CourseEnrollmentsVM
            {
                CoursesNotEnrolledByEmployee = coursesNotEnrolledByEmployee
            };

            foreach (var course in coursesNotEnrolledByEmployee)
            {
                coursesNotEnrolled.CoursesToEnrol.Add(course, false);
            }
            return coursesNotEnrolled;
        }
        public async Task<ICollection<CourseEnrollmentsVM>> GetAllEmployeeCourseEnrollments(int? employeeId)
        {
            var employeeCourses = await _dbContext.CourseEnrollments
                 .Include(x => x.Employee)
                 .Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                 .Where(x => x.EmployeeId == employeeId)
                 .ProjectTo<CourseEnrollmentsVM>(_configurationProvider)
                 .ToListAsync();
            return employeeCourses;
        }
        public async Task<ICollection<CourseEnrollmentsVM>> GetAllCoursesCompletedByEmployee(int? courseId)
        {
            var totalCompletedCourses = await _dbContext.CourseEnrollments
               .Where(x => x.CourseId == courseId && x.Status.Equals("Completed"))
               .ProjectTo<CourseEnrollmentsVM>(_configurationProvider)
               .ToListAsync();
            return totalCompletedCourses;
        }

        public async Task<ICollection<CourseEnrollmentsVM>> GetAllCoursesInProcessByEmployee(int? courseId)
        {
            var totalInProccessCourses = await _dbContext.CourseEnrollments
                .Where(x => x.CourseId == courseId && x.Status.Equals("In proccess"))
                .ProjectTo<CourseEnrollmentsVM>(_configurationProvider)
                .ToListAsync();
            return totalInProccessCourses;
        }
        public async Task<bool> EnrolEmployeeToCourse(int employeeId, CourseEnrollmentsVM courseEnrollmentsVM)
        {
            //Get all the courses the user selected from dictionary object
            var coursesSelected = courseEnrollmentsVM.CoursesToEnrol.Where(x => x.Value is true).Select(x => x.Key);
            if (coursesSelected.Count() is 0)
            {
                return false;
            }

            /*
              From the courses the user selected, get all the courses from the courses table
              and only get the ones the user selected so that we can get the Ids of those courses
             */
            var allCourses = await _coursesRepository.GetAllCourses();
            var CoursesToGetCourseIds = allCourses.Where(x => coursesSelected.Any(y => x.CourseName.Contains(y)));

            var coursesToEnrolToEmployee = new List<CourseEnrollmentsVM>();
            foreach (var course in CoursesToGetCourseIds)
            {
                coursesToEnrolToEmployee.Add(new CourseEnrollmentsVM()
                {
                    CourseId = course.Id,
                    EmployeeId = employeeId,
                    Status = "Enrolled"
                });
            }
            var EmployeeCourseEnrollments = _mapper.Map<ICollection<CourseEnrollment>>(coursesToEnrolToEmployee);
            await _dbContext.CourseEnrollments.AddRangeAsync(EmployeeCourseEnrollments);
            return true;
        }

        public async Task<int> GetTotalNumberOfCoursesEnrolledByEmployee(int employeeId)
        {
            var totalNoOfCoursesEnrolled = await _dbContext.CourseEnrollments
                 .Where(x => x.EmployeeId == employeeId).CountAsync();
            return totalNoOfCoursesEnrolled;
        }

        public async Task<int> GetTotalOfAllEnrollments()
        {
            var totalEnrollments = await _dbContext.CourseEnrollments.CountAsync();
            return totalEnrollments;
        }

        public async Task UnenrollEmployeeFromCourse(int courseEnrollmentId)
        {
            var CourseEnrollment = await _dbContext.CourseEnrollments.Where(x => x.Id == courseEnrollmentId)
                //Just to observe if we are deleting the right course enrollment for debugging
                .Include(x => x.Course)
                .Include(x => x.Employee)
                .FirstOrDefaultAsync();
            _dbContext.CourseEnrollments.Remove(CourseEnrollment);
        }

        public async Task<int> GetTotalNumberOfEmployeesEnrolledInCourse(int courseId)
        {
            var totalCourseEnrollments = await _dbContext.CourseEnrollments
                .CountAsync(x => x.CourseId == courseId);
            return totalCourseEnrollments;
        }

        public async Task<int> GetTotalCoursesCompletedByEmployees(int courseId)
        {
            var totalCompletedCourses = await _dbContext.CourseEnrollments
                 .CountAsync(x => x.CourseId == courseId && x.Status.Equals("Completed"));
            return totalCompletedCourses;
        }

        public async Task<int> GetTotalCoursesInProccessByEmployees(int courseId)
        {
            var totalInProccessCourses = await _dbContext.CourseEnrollments
                 .CountAsync(x => x.CourseId == courseId && x.Status.Equals("In proccess"));
            return totalInProccessCourses;
        }

        public async Task<int> GetTotalCoursesCompletedByEmployee(int employeeId)
        {
            var totalCompletedCourses = await _dbContext.CourseEnrollments
                .Where(x => x.EmployeeId == employeeId)
                .CountAsync(x => x.Status.Equals("Completed"));
            return totalCompletedCourses;
        }

        public async Task<int> GetTotalCoursesInProccessByEmployee(int employeeId)
        {
            var totalCoursesInProcess = await _dbContext.CourseEnrollments
                .Where(x => x.EmployeeId == employeeId)
                .CountAsync(x => x.Status.Equals("In proccess"));
            return totalCoursesInProcess;
        }

        public async Task<ICollection<CourseEnrollmentsVM>> GetAllEmployeesEnrolledInCourse(int? courseId)
        {
            var employeesInCourse = await _dbContext.CourseEnrollments
                 .Where(x => x.CourseId == courseId)
                 .Include(x => x.Employee)
                 .ProjectTo<CourseEnrollmentsVM>(_configurationProvider)
                 .ToListAsync();
            return employeesInCourse;
        }
        public async Task UpdateEmployeeCourseEnrollment(CourseEnrollmentsVM courseEnrollmentsVM)
        {
            var employeeEnrollmentModel = _mapper.Map<CourseEnrollment>(courseEnrollmentsVM);
            _dbContext.CourseEnrollments.Update(employeeEnrollmentModel);
        }

        public async Task<ExcelFileDownloadProperties> GetAllEmployeesEnrolledInCourseToExcel(int courseId)
        {
            var courseEnrollmentsData = await GetAllEmployeesEnrolledInCourse(courseId);
            var courseEnrollmentsExcelFile = await _excelFileService.GenerateExcelFileFromCourseEnrollmentsData(courseEnrollmentsData);
            return courseEnrollmentsExcelFile;

        }
        public async Task<ExcelFileDownloadProperties> GetAllEmployeesWhoCompletedCourseToExcel(int courseId)
        {
            var employeesWhoCompletedCourses = await GetAllCoursesCompletedByEmployee(courseId);
            var employeesWhoCompletedCoursesData = await _excelFileService.GenerateExcelFileFromCourseEnrollmentsData(employeesWhoCompletedCourses);
            return employeesWhoCompletedCoursesData;
        }

        public async Task<ExcelFileDownloadProperties> GetAllEmployeesInProccessToExcel(int courseId)
        {
            var employeesInProccess = await GetAllCoursesInProcessByEmployee(courseId);
            var employeesInProccessData = await _excelFileService.GenerateExcelFileFromCourseEnrollmentsData(employeesInProccess);
            return employeesInProccessData;
        }

        public async Task<ExcelFileDownloadProperties> GetAllEmployeeCourseEnrollmentsToExcel(int employeeId)
        {
            var employeeCourseEnrollments = await GetAllEmployeeCourseEnrollments(employeeId);
            var employeeCourseEnrollmentsData = await _excelFileService.GenerateExcelFileForEmployeeCourseEnrollments(employeeCourseEnrollments);
            return employeeCourseEnrollmentsData;

        }
    }
}
