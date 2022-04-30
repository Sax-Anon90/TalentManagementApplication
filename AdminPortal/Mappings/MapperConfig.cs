using AdminPortal.Data;
using AdminPortal.Models;
using AutoMapper;

namespace AdminPortal.Mappings
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Department, DepartmentsVM>().ReverseMap();
            CreateMap<Course, CoursesVM>().ReverseMap();
            CreateMap<CourseCategory, CourseCategoryVM>().ReverseMap();
        }
    }
}
