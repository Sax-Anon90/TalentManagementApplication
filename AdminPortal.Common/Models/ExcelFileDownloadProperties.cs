using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Common.Models
{
    public class ExcelFileDownloadProperties
    {
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public byte[]? FileContent { get; set; }
    }
}
