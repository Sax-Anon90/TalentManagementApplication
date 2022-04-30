using AdminPortal.Data;

namespace AdminPortal.Models
{
    public class CourseCategoryVM
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<CoursesVM>? Courses { get; set; }
    }
}
