using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Common.Models.CoursesViewModels
{
    public class CourseFileAttachmentViewVM : BaseViewModel.BaseViewModel
    {
        [Display(Name="File Name")]
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public byte[]? FileContent { get; set; }
        public int CourseId { get; set; }
    }
}
