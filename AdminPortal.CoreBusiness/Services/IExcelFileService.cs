using AdminPortal.Common.Models;
using AdminPortal.Common.Models.CoursesViewModels;
using AdminPortal.Common.Models.EmployeesViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Services
{
    public interface IExcelFileService
    {
        Task<ExcelFileDownloadProperties> GenerateExcelFileFromEmployeeData(ICollection<EmployeeVM> employeeData);
        Task<ExcelFileDownloadProperties> GenerateExcelFileFromCourseEnrollmentsData(ICollection<CourseEnrollmentsVM> courseEnrollmentsData);
        Task<ExcelFileDownloadProperties> GenerateExcelFileFromCoursesData(ICollection<CoursesVM> coursesData);
        Task<ExcelFileDownloadProperties> GenerateExcelFileForEmployeeCourseEnrollments(ICollection<CourseEnrollmentsVM> EmployeecourseEnrollmentsData);
    }
}