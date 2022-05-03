using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Common.Models
{
    public class CourseCreateViewModel
    {
        [Display(Name="Course Name")]
        public string? CourseName { get; set; }
        [Display(Name = "Select course category")]
        public int? CourseCategoryId { get; set; }
        public SelectList? CourseCategories { get; set; }
        public bool Validation()
        {
            if(CourseName == null || CourseCategoryId == null || CourseCategoryId == 0)
            {
                return false;
            }
            return true;
        }
    }
}
