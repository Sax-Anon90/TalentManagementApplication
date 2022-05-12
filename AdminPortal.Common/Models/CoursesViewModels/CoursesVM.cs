using AdminPortal.Common.Models.CoursesViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Common.Models.CoursesViewModels
{
    public class CoursesVM : BaseViewModel.BaseViewModel
    {
        [Display(Name = "Course Name")]
        public string? CourseName { get; set; }

        [Display(Name = "Select course category")]
        public int? CourseCategoryId { get; set; }

        //For course update only
        public SelectList? CourseCategories { get; set; }

        public virtual CourseCategoryVM? CourseCategory { get; set; }

        public bool Validation()
        {
            if (Id == 0 || CourseName is null || CourseCategoryId is null || CourseCategoryId == 0)
            {
                return false;
            }
            return true;
        }
    }
}
