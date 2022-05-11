using AdminPortal.Data.Data;
using AdminPortal.Common.Models;
using AutoMapper;
using AdminPortal.Common.Models.CoursesViewModels;
using AdminPortal.Common.Models.EmployeesViewModels;

namespace AdminPortal.CoreBusiness.Mappings
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Department, DepartmentsVM>().ReverseMap();
            CreateMap<Course, CoursesVM>().ReverseMap();
            CreateMap<CourseCategory, CourseCategoryVM>().ReverseMap();
            CreateMap<CourseFileAttachment, CourseFileAttachmentViewVM>().ReverseMap();
            CreateMap<Employee, EmployeeVM>().ReverseMap();
            CreateMap<Employee, EmployeeSearchResultVM>().ReverseMap();
            CreateMap<Employee, EmployeeCreateVM>().ReverseMap();
            CreateMap<EmployeeFileAttachment, EmployeeFileAttachmentsVM>().ReverseMap();
            CreateMap<CourseEnrollment, CourseEnrollmentsVM>().ReverseMap();
            CreateMap<CourseEnrollment, ICollection<CourseEnrollmentsVM>>().ReverseMap();
            CreateMap<Department, DepartmentsVM>().ReverseMap();
            CreateMap<Gender, GenderVM>().ReverseMap();
        }
    }
}
