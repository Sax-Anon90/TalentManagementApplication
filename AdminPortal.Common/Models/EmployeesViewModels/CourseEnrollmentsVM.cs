using AdminPortal.Common.Models.CoursesViewModels;

namespace AdminPortal.Common.Models.EmployeesViewModels
{
    //Employees enrolled in a course hence class is in EmployeesViewModels folder and not CoursesViewModel
    public class CourseEnrollmentsVM : BaseViewModel.BaseViewModel
    {
        public string? Status { get; set; }
        public int? EmployeeId { get; set; }
        public int? CourseId { get; set; }
        public virtual CoursesVM? Course { get; set; }
        public virtual EmployeeVM? Employee { get; set; }


        //To only display the courses the employee has not enrolled in yet
        public IEnumerable<string> CoursesNotEnrolledByEmployee { get; set; } = new List<string>();

        //To populate all courses in "CoursesToEnrol" object and use it for checkbox selection
        public Dictionary<string, bool> CoursesToEnrol { get; set; } = new Dictionary<string, bool>();

        public bool Validation()    
        {
            if (Status is null || EmployeeId is null || EmployeeId is 0 || CourseId is null || CourseId is 0)
            {
                return false;
            }
            return true;
        }

    }
}