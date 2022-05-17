using AdminPortal.Common.Models;
using AdminPortal.Common.Models.EmployeesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Repositories.Contracts
{
    public interface ICourseEnrollmentsRepository
    {
        Task<ICollection<CourseEnrollmentsVM>> GetAllEmployeeCourseEnrollments(int? employeeId);
        Task<ICollection<CourseEnrollmentsVM>> GetAllCoursesCompletedByEmployee(int? courseId);
        Task<ICollection<CourseEnrollmentsVM>> GetAllCoursesInProcessByEmployee(int? courseId);
        Task<ICollection<CourseEnrollmentsVM>> GetAllEmployeesEnrolledInCourse(int? courseId);
        Task<CourseEnrollmentsVM> GetEmployeeCourseEnrollment(int employeeCourseEnrollmentId);
        Task<bool> EnrolEmployeeToCourse(int employeeId, CourseEnrollmentsVM courseEnrollmentsVM);
        Task<CourseEnrollmentsVM> GetCoursesNotEnrolledByEmployee(int? employeeId);
        Task UnenrollEmployeeFromCourse(int courseEnrollmentId);
        Task UpdateEmployeeCourseEnrollment(CourseEnrollmentsVM courseEnrollmentsVM);


        //Return numbers
        Task<int> GetTotalNumberOfCoursesEnrolledByEmployee(int employeeId);
        Task<int> GetTotalCoursesCompletedByEmployee(int employeeId);
        Task<int> GetTotalCoursesCompletedByEmployees(int courseId);
        Task<int> GetTotalCoursesInProccessByEmployee(int employeeId);
        Task<int> GetTotalCoursesInProccessByEmployees(int courseId);
        Task<int> GetTotalOfAllEnrollments();
        Task<int> GetTotalNumberOfEmployeesEnrolledInCourse(int courseId);

        //Excel files
        Task<ExcelFileDownloadProperties> GetAllEmployeesEnrolledInCourseToExcel(int courseId);
        Task<ExcelFileDownloadProperties> GetAllEmployeesWhoCompletedCourseToExcel(int courseId);
        Task<ExcelFileDownloadProperties> GetAllEmployeesInProccessToExcel(int courseId);
    }
}
