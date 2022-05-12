using AdminPortal.Common.Models.BaseViewModel;
using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Common.Models.CoursesViewModels
{
    public class CourseCategoryVM : BaseViewModel.BaseViewModel
    {
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
