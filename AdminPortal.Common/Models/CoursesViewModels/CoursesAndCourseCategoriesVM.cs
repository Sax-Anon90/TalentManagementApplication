
namespace AdminPortal.Common.Models.CoursesViewModels
{
    public class CoursesAndCourseCategoriesVM
    {
        public ICollection<CoursesVM> CoursesViewModel { get; set; } = new List<CoursesVM>();
        public ICollection<CourseCategoryVM> CourseCategoriesViewModel { get; set; } = new List<CourseCategoryVM>();
        public CourseCreateViewModel CourseCreateViewModel { get; set; } = new CourseCreateViewModel();
        public CourseFileAttachmentVM CourseFileAttachmentVM { get; set; } = new CourseFileAttachmentVM();
        public CourseCategoryVM courseCategoryCreateVM { get; set; } = new CourseCategoryVM();  

    }
}
