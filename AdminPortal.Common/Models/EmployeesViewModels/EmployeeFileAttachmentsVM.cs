using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models.EmployeesViewModels
{
    public class EmployeeFileAttachmentsVM
    {
        public int Id { get; set; }
        [Display(Name="File Name")]
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public byte[]? FileContent { get; set; }
        public int? EmployeeId { get; set; }
    }
}
