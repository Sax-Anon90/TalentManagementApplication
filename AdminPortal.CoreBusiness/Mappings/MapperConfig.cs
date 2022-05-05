using AdminPortal.Data.Data;
using AdminPortal.Common.Models;
using AutoMapper;

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
        }
    }
}
