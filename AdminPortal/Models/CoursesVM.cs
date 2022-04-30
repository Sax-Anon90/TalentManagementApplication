using AdminPortal.Data;

namespace AdminPortal.Models
{
    public class CoursesVM
    {
        public int Id { get; set; }
        public string? CourseName { get; set; }
        public int? CourseCategoryId { get; set; }

        public virtual CourseCategoryVM? CourseCategory { get; set; }
    }
}
