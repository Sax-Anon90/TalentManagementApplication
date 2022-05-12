#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdminPortal.Data.Data;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Common.Models.EmployeesViewModels;

namespace AdminPortal.UI.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var EmployeesAndEmployeeTrainingModel = new EmployeeAndEmployeeTrainingVM()
            {
                EmployeesViewModel = await _unitOfWork.EmployeeRepository.GetAllEmployees()
            };
            return View(EmployeesAndEmployeeTrainingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewEmployee(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM)
        {
            var validationResult = employeeAndEmployeeTrainingVM.EmployeeCreateViewModel.Validation();
            if (validationResult is false)
            {
                TempData["EmployeeeAddFail"] = "Employee not added. Please ensure that you enter all required details. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }
            await _unitOfWork.EmployeeRepository.AddNewEmployee(employeeAndEmployeeTrainingVM);
            await _unitOfWork.SaveChangesAsync();
            TempData["EmployeeAddSuccess"] = "Employee successfully added";
            return RedirectToAction(nameof(Index), "Employees");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadEmployeeFileAttachment(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM, int employeeId)
        {
            var result = await _unitOfWork.EmployeeFileAttachmentsRepository.UploadEmployeeFileAttachment(employeeAndEmployeeTrainingVM, employeeId);
            await _unitOfWork.SaveChangesAsync();
            if (result == false)
            {
                TempData["EmployeeFileUploadFail"] = "Employee File Upload failed. Please ensure that you have entered a valid file. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }
            TempData["EmployeeFileUploadSuccess"] = "Employee File Uploaded successfully";
            return RedirectToAction(nameof(Index), "Employees");
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeDetailsView(int employeeId)
        {
            var EmployeeDetailsAndEmployeeTrainingModel = new EmployeeDetailsAndEmployeeFilesVM()
            {
                EmployeeDetails = await _unitOfWork.EmployeeRepository.GetEmployeeDetails(employeeId),
                EmployeeCourseEnrollmentsVM = await _unitOfWork.CoursesEnrollmentsRepository.GetAllEmployeeCourseEnrollments(employeeId),
                EmployeeFileAttachmentsVM = await _unitOfWork.EmployeeFileAttachmentsRepository.GetAllEmployeeFileAttachmentsById(employeeId)
            };
            return View("Details", EmployeeDetailsAndEmployeeTrainingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchEmployee(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM)
        {
            if (employeeAndEmployeeTrainingVM.EmployeeSearchResult.EmployeeNo is null)
            {
                TempData["SearchResultFail"] = "Please enter an employee number";
                return RedirectToAction(nameof(Index), "Employees");
            }
            var employeeNo = employeeAndEmployeeTrainingVM.EmployeeSearchResult.EmployeeNo.ToString();
            var searchResult = await _unitOfWork.EmployeeRepository.SearchEmployeeByEmployeeNo(employeeNo);
            if (searchResult == null)
            {
                TempData["SearchResultFail"] = "There are no results found. Please try again";
            }
            else
            {
                TempData["SearchResultSuccess"] = "Search Result complete. Click on the Employee Trainings tab to see the result";
            }
            var EmployeesAndEmployeeTrainingModel = new EmployeeAndEmployeeTrainingVM()
            {
                EmployeesViewModel = await _unitOfWork.EmployeeRepository.GetAllEmployees(),
                EmployeeSearchResult = searchResult
            };
            return View(nameof(Index), EmployeesAndEmployeeTrainingModel);
        }

        [HttpGet]
        public async Task<IActionResult> EnrolEmployeeToCourseView(int? employeeId)
        {
            var coursesToEnrol = await _unitOfWork.CoursesEnrollmentsRepository.GetCoursesNotEnrolledByEmployee(employeeId);
            if (coursesToEnrol == null)
            {
                TempData["AllCoursesEnrolled"] = "This employee has enrolled in all courses";
            }
            else
            {
                coursesToEnrol.EmployeeId = employeeId;
            }
            return View("EnrolEmployeeToCourseView", coursesToEnrol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrolEmployeeToCourse(int employeeId, CourseEnrollmentsVM courseEnrollmentsVM)
        {
            var result = await _unitOfWork.CoursesEnrollmentsRepository.EnrolEmployeeToCourse(employeeId, courseEnrollmentsVM);
            await _unitOfWork.SaveChangesAsync();
            if (result is false)
            {
                TempData["EnrolEmployeeToCourseFail"] = "Employee not enrolled to courses. Please ensure you select a course." +
                    "If the problem persists, contact the systems administrator";
                return View(nameof(Index), "Employees");
            }
            TempData["EnrolEmployeeToCourseSucess"] = "Employee successfully enrolled to selected courses";
            return RedirectToAction(nameof(Index), "Employees");
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateEmployeePopUpView(int? employeeId)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetEmployeeDetails(employeeId);
            return PartialView("_UpdateEmployeePopUpView", employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(EmployeeVM employeeUpdateVM)
        {
            var validationResult = employeeUpdateVM.Valdiation();
            if (validationResult is false)
            {
                TempData["UpdateEmployeeFail"] = "Employee Update Failed. Please ensure that you enter all required details. If the problem persists, contact the systems administrator";
                RedirectToAction(nameof(Index), "Employees");
            }
            await _unitOfWork.EmployeeRepository.UpdateEmployeeDetails(employeeUpdateVM);
            await _unitOfWork.SaveChangesAsync();
            TempData["EmplyeeUpdateSuccess"] = "Employee successfully updated";
            return RedirectToAction(nameof(Index), "Employees");
        }

        [HttpGet]
        public async Task<FileResult> DownloadEmployeeFileAttachment(int Id)
        {
            var EmployeeFileAttachment = await _unitOfWork.EmployeeFileAttachmentsRepository.GetEmployeeFileAttachment(Id);
            var fileName = EmployeeFileAttachment.FileName;
            var fileType = EmployeeFileAttachment.FileType;
            var fileContent = EmployeeFileAttachment.FileContent;
            return File(fileContent, fileType, fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeFileAttachment(int employeeFileAttachmentId)
        {
            await _unitOfWork.EmployeeFileAttachmentsRepository.DeleteEmployeeFileAttachment(employeeFileAttachmentId);
            await _unitOfWork.SaveChangesAsync();
            TempData["EmployeeFileDeleteSuccess"] = "Employee File Attachment successfully deleted";
            return RedirectToAction(nameof(Index), "Employees");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee(string employeeNo)
        {
            await _unitOfWork.EmployeeRepository.DeleteEmployee(employeeNo);
            await _unitOfWork.SaveChangesAsync();
            TempData["EmployeeDeleteSuccess"] = "Employee Successfully Deleted";
            return RedirectToAction(nameof(Index), "Employees");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnenrollEmployeeFromCourse(int courseEnrollmentId)
        {
            await _unitOfWork.CoursesEnrollmentsRepository.UnenrollEmployeeFromCourse(courseEnrollmentId);
            await _unitOfWork.SaveChangesAsync();
            TempData["EmployeeEnrollmentDeleteSuccess"] = "Employee unenrolled from course";
            return RedirectToAction(nameof(Index), "Employees");
        }

        [HttpGet]
        public async Task<FileResult> DownloadTotalEmployeesToExcel()
        {
            var employeeExcelFile = await _unitOfWork.EmployeeRepository.GenerateExcelFileForTotalEmployees();
            var fileName = employeeExcelFile.FileName;
            var fileType = employeeExcelFile.FileType;
            var fileContent = employeeExcelFile.FileContent;
            return File(fileContent, fileType, fileName);
        }

    }
}
