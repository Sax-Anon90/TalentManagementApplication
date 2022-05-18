using AdminPortal.Common.Models;
using AdminPortal.Common.Models.CoursesViewModels;
using AdminPortal.Common.Models.EmployeesViewModels;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.CoreBusiness.Services
{
    public class ExcelFileService : IExcelFileService
    {
        public async Task<ExcelFileDownloadProperties> GenerateExcelFileForEmployeeCourseEnrollments(ICollection<CourseEnrollmentsVM> EmployeecourseEnrollmentsData)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExcelFileResult");
                int currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Course";
                worksheet.Cell(currentRow, 2).Value = "CourseCategory";
                worksheet.Cell(currentRow, 3).Value = "Status";

                foreach (var data in EmployeecourseEnrollmentsData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.Course.CourseName;
                    worksheet.Cell(currentRow, 2).Value = data.Course.CourseCategory.CategoryName;
                    worksheet.Cell(currentRow, 3).Value = data.Status;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var ExcelFile = new ExcelFileDownloadProperties()
                    {
                        FileName = "ExcelFileResult.xlsx",
                        FileType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet",
                        FileContent = content
                    };
                    return ExcelFile;
                }
            }
        }

        public async Task<ExcelFileDownloadProperties> GenerateExcelFileFromCourseEnrollmentsData(ICollection<CourseEnrollmentsVM> courseEnrollmentsData)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExcelFileResult");
                int currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Employee No";
                worksheet.Cell(currentRow, 2).Value = "First Name";
                worksheet.Cell(currentRow, 3).Value = "Last Name";
                worksheet.Cell(currentRow, 4).Value = "Gender";
                worksheet.Cell(currentRow, 5).Value = "Department";
                worksheet.Cell(currentRow, 6).Value = "Position Title";


                foreach (var data in courseEnrollmentsData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.Employee.EmployeeNo;
                    worksheet.Cell(currentRow, 2).Value = data.Employee.FirstName;
                    worksheet.Cell(currentRow, 3).Value = data.Employee.LastName;
                    worksheet.Cell(currentRow, 4).Value = data.Employee.Gender;
                    worksheet.Cell(currentRow, 5).Value = data.Employee.Department;
                    worksheet.Cell(currentRow, 6).Value = data.Employee.PositionTitle;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var ExcelFile = new ExcelFileDownloadProperties()
                    {
                        FileName = "ExcelFileResult.xlsx",
                        FileType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet",
                        FileContent = content
                    };
                    return ExcelFile;
                }
            }
        }

        public  async Task<ExcelFileDownloadProperties> GenerateExcelFileFromCoursesData(ICollection<CoursesVM> coursesData)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExcelFileResult");
                int currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Course Name";
                worksheet.Cell(currentRow, 2).Value = "Course Category";

                foreach (var data in coursesData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.CourseName;
                    worksheet.Cell(currentRow, 2).Value = data.CourseCategory.CategoryName;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var ExcelFile = new ExcelFileDownloadProperties()
                    {
                        FileName = "ExcelFileResult.xlsx",
                        FileType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet",
                        FileContent = content
                    };
                    return ExcelFile;
                }
            }
        }

        public async Task<ExcelFileDownloadProperties> GenerateExcelFileFromEmployeeData(ICollection<EmployeeVM> employeeData)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("ExcelFileResult");
                int currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Employee No";
                worksheet.Cell(currentRow, 2).Value = "First Name";
                worksheet.Cell(currentRow, 3).Value = "Last Name";
                worksheet.Cell(currentRow, 4).Value = "Gender";
                worksheet.Cell(currentRow, 5).Value = "Department";
                worksheet.Cell(currentRow, 6).Value = "Position Title";

                foreach (var data in employeeData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = data.EmployeeNo;
                    worksheet.Cell(currentRow, 2).Value = data.FirstName;
                    worksheet.Cell(currentRow, 3).Value = data.LastName;
                    worksheet.Cell(currentRow, 4).Value = data.Gender;
                    worksheet.Cell(currentRow, 5).Value = data.Department;
                    worksheet.Cell(currentRow, 6).Value = data.PositionTitle;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var ExcelFile = new ExcelFileDownloadProperties()
                    {
                        FileName = "ExcelFileResult.xlsx",
                        FileType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet",
                        FileContent = content
                    };
                    return ExcelFile;
                }
            }
        }
    }
}
