using System;
using System.Collections.Generic;

namespace AdminPortal.Data
{
    public partial class EmployeeFileAttachment
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public byte[]? FileContent { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
