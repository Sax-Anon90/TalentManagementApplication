using System;
using System.Collections.Generic;

namespace AdminPortal.Data
{
    public partial class CourseFileAttachment
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public byte[]? FileContent { get; set; }
        public int? CourseId { get; set; }

        public virtual Course? Course { get; set; }
    }
}
