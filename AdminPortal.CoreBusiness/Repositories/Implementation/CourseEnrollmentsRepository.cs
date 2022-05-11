using AdminPortal.Common.Models.EmployeesViewModels;
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
    public class CourseEnrollmentsRepository : ICourseEnrollmentsRepository
    {
        private readonly SaxTalentManagementContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly ICoursesRepository _coursesRepository;
        public CourseEnrollmentsRepository(SaxTalentManagementContext _dbContext, IMapper _mapper,
            AutoMapper.IConfigurationProvider _configurationProvider, ICoursesRepository _coursesRepository)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
            this._configurationProvider = _configurationProvider;
            this._coursesRepository = _coursesRepository;
        }
        public async Task<CourseEnrollmentsVM> GetCoursesNotEnrolledByEmployee(int? employeeId)
        {
            var allCourses = await _coursesRepository.GetAllCourses();
            var courseEnrollments = GetAllEmployeeCourseEnrollments(employeeId);
            /*
              From all the courses in the courses table, select only the one's the employee has not yet enrolled in
             */
            var coursesNotEnrolledByEmployee = allCourses.Select(x => x.CourseName)
                .Where(x => !courseEnrollments.Result.Select(x => x.Course.CourseName).Any(y => x.Contains(y)));

            //This means the employee has enrolled in all courses
            if (coursesNotEnrolledByEmployee.Count() is 0)
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
                 .Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                 .Where(x => x.EmployeeId == employeeId)
                 .ProjectTo<CourseEnrollmentsVM>(_configurationProvider)
                 .ToListAsync();
            return employeeCourses;
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
    }
}
