using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Common.Models
{
    public class CourseCategoryVM
    {
        public int Id { get; set; }
        [Display(Name = "Category Name")]
        public string? CategoryName { get; set; }

        public virtual ICollection<CoursesVM>? Courses { get; set; }

        public bool validation()
        {
            if (CategoryName == null)
            {
                return false;
            }
            return true;
        }
    }
}
