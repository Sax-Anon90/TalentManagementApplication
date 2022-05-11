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
        Task<bool> EnrolEmployeeToCourse(int employeeId, CourseEnrollmentsVM courseEnrollmentsVM);
        Task<CourseEnrollmentsVM> GetCoursesNotEnrolledByEmployee(int? employeeId);
    }
}
