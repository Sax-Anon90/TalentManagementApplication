namespace AdminPortal.Models
{
    public class CoursesAndCourseCategoriesVM
    {
        public ICollection<CoursesVM> CoursesViewModel { get; set; } = new List<CoursesVM>();
        public ICollection<CourseCategoryVM> CourseCategoriesViewModel { get; set; } = new List<CourseCategoryVM>();
    }
}
