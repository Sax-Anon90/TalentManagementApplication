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
using AdminPortal.UI.Constants;
using Microsoft.AspNetCore.Authorization;

namespace AdminPortal.UI.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IUnitOfWork _unitOfWork,
            ILogger<EmployeesController> _logger)
        {
            this._unitOfWork = _unitOfWork;
            this._logger = _logger;
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin + "," + Roles.Viewer)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var allEmployees = await _unitOfWork.EmployeeRepository.GetAllEmployees();
                var EmployeesAndEmployeeTrainingModel = new EmployeeAndEmployeeTrainingVM()
                {
                    EmployeesViewModel = allEmployees
                };
                return View("Index", EmployeesAndEmployeeTrainingModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to get all employee data");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                    "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewEmployee(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM)
        {
            try
            {
                var validationResult = employeeAndEmployeeTrainingVM.EmployeeCreateViewModel.Validation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in trying to add a new employee: Not all values were entered");
                    TempData["EmployeeeAddFail"] = "Employee not added. Please ensure that you enter all required details. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Employees");
                }
                await _unitOfWork.EmployeeRepository.AddNewEmployee(employeeAndEmployeeTrainingVM);
                await _unitOfWork.SaveChangesAsync();
                TempData["EmployeeAddSuccess"] = "Employee successfully added";
                return RedirectToAction(nameof(Index), "Employees");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to add a new employee");
                TempData["EmployeeeAddFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadEmployeeFileAttachment(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM, int employeeId)
        {
            try
            {
                var result = await _unitOfWork.EmployeeFileAttachmentsRepository.UploadEmployeeFileAttachment(employeeAndEmployeeTrainingVM, employeeId);
                if (result == false)
                {
                    _logger.LogError("Error in trying to upload an employee File: Invalid file may have been entered");
                    TempData["EmployeeFileUploadFail"] = "Employee File Upload failed. Please ensure that you have entered a valid file. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Employees");
                }
                await _unitOfWork.SaveChangesAsync();
                TempData["EmployeeFileUploadSuccess"] = "Employee File Uploaded successfully";
                return RedirectToAction(nameof(Index), "Employees");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to upload an employee File.");
                TempData["EmployeeFileUploadFail"] = "Something went wrong. Please try again.If the problem persists, Please contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin + "," + Roles.Viewer)]
        [HttpGet]
        public async Task<IActionResult> EmployeeDetailsView(int employeeId)
        {
            try
            {
                var EmployeeDetailsAndEmployeeTrainingModel = new EmployeeDetailsAndEmployeeFilesVM()
                {
                    EmployeeDetails = await _unitOfWork.EmployeeRepository.GetEmployeeDetails(employeeId),
                    EmployeeCourseEnrollmentsVM = await _unitOfWork.CoursesEnrollmentsRepository.GetAllEmployeeCourseEnrollments(employeeId),
                    EmployeeFileAttachmentsVM = await _unitOfWork.EmployeeFileAttachmentsRepository.GetAllEmployeeFileAttachmentsById(employeeId)
                };
                return View("EmployeeDetailsView", EmployeeDetailsAndEmployeeTrainingModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to retrieve employee details data");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                   "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchEmployee(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM)
        {
            try
            {
                if (employeeAndEmployeeTrainingVM.EmployeeSearchResult.EmployeeNo is null)
                {
                    _logger.LogError("Erorr in searching for employee: No employee number was entered");
                    TempData["SearchResultFail"] = "Please enter an employee number";
                    return RedirectToAction(nameof(Index), "Employees");
                }
                var employeeNo = employeeAndEmployeeTrainingVM.EmployeeSearchResult.EmployeeNo.ToString();
                var searchResult = await _unitOfWork.EmployeeRepository.SearchEmployeeByEmployeeNo(employeeNo);
                if (searchResult == null)
                {
                    _logger.LogError("Erorr in searching for employee: No results found");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erorr in searching for employee");
                TempData["SearchResultFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> EnrolEmployeeToCourseView(int? employeeId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erorr in enrolling employees to course view for employee");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrolEmployeeToCourse(int employeeId, CourseEnrollmentsVM courseEnrollmentsVM)
        {
            try
            {
                var result = await _unitOfWork.CoursesEnrollmentsRepository.EnrolEmployeeToCourse(employeeId, courseEnrollmentsVM);
                if (result is false)
                {
                    _logger.LogError("Erorr in enrolling employees to course: No courses were selected");
                    TempData["EnrolEmployeeToCourseFail"] = "Employee not enrolled to courses. Please ensure you select a course." +
                        "If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Employees");
                }
                await _unitOfWork.SaveChangesAsync();
                TempData["EnrolEmployeeToCourseSucess"] = "Employee successfully enrolled to selected courses";
                return RedirectToAction(nameof(Index), "Employees");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in enrolling employees to course:");
                TempData["EnrolEmployeeToCourseFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> UpdateEmployeePopUpView(int? employeeId)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetEmployeeDetails(employeeId);
                return PartialView("_UpdateEmployeePopUpView", employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Employee Update Pop View");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployee(EmployeeVM employeeUpdateVM)
        {
            try
            {
                var validationResult = employeeUpdateVM.Valdiation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in Employee Update: Incorrect Validation");
                    TempData["UpdateEmployeeFail"] = "Employee Update Failed. Please ensure that you enter all required details. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Employees");
                }
                await _unitOfWork.EmployeeRepository.UpdateEmployeeDetails(employeeUpdateVM);
                await _unitOfWork.SaveChangesAsync();
                TempData["EmplyeeUpdateSuccess"] = "Employee successfully updated";
                return RedirectToAction(nameof(Index), "Employees");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Employee Update");
                TempData["UpdateEmployeeFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadEmployeeFileAttachment(int Id)
        {
            try
            {
                var EmployeeFileAttachment = await _unitOfWork.EmployeeFileAttachmentsRepository.GetEmployeeFileAttachment(Id);
                var fileName = EmployeeFileAttachment.FileName;
                var fileType = EmployeeFileAttachment.FileType;
                var fileContent = EmployeeFileAttachment.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Downloading employee File Attachment");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeFileAttachment(int employeeFileAttachmentId, int employeeIdForDetails)
        {
            try
            {
                await _unitOfWork.EmployeeFileAttachmentsRepository.DeleteEmployeeFileAttachment(employeeFileAttachmentId);
                await _unitOfWork.SaveChangesAsync();
                TempData["EmployeeFileDeleteSuccess"] = "Employee File Attachment successfully deleted";
                return RedirectToAction(nameof(EmployeeDetailsView), "Employees", new { employeeId = employeeIdForDetails });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Deleting employee File Attachment");
                TempData["EmployeeFileDeleteFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(EmployeeDetailsView), "Employees", new { employeeId = employeeIdForDetails });
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee(string employeeNo)
        {
            try
            {
                await _unitOfWork.EmployeeRepository.DeleteEmployee(employeeNo);
                await _unitOfWork.SaveChangesAsync();
                TempData["EmployeeDeleteSuccess"] = "Employee Successfully Deleted";
                return RedirectToAction(nameof(Index), "Employees");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Deleting employee");
                TempData["EmployeeDeleteFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnenrollEmployeeFromCourse(int courseEnrollmentId, int employeeId)
        {
            try
            {
                await _unitOfWork.CoursesEnrollmentsRepository.UnenrollEmployeeFromCourse(courseEnrollmentId);
                await _unitOfWork.SaveChangesAsync();
                TempData["EmployeeEnrollmentDeleteSuccess"] = "Employee unenrolled from course";
                return RedirectToAction(nameof(EmployeeDetailsView), "Employees", new { employeeId = employeeId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Unenrolling employee from a course");
                TempData["EmployeeEnrollmentDeleteFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(EmployeeDetailsView), "Employees", new { employeeId = employeeId });
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadTotalEmployeesToExcel()
        {
            try
            {
                var employeeExcelFile = await _unitOfWork.EmployeeRepository.GenerateExcelFileForTotalEmployees();
                var fileName = employeeExcelFile.FileName;
                var fileType = employeeExcelFile.FileType;
                var fileContent = employeeExcelFile.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Downloading all employees to excel");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> UpdateEmployeeCourseEnrollmentView(int employeeCourseEnrollmentId)
        {
            try
            {
                var employeeCourseEnrollment = await _unitOfWork.CoursesEnrollmentsRepository.GetEmployeeCourseEnrollment(employeeCourseEnrollmentId);
                return PartialView("_UpdateEmployeeCourseEnrollmentView", employeeCourseEnrollment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Update Employee Enrollment View");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployeeCourseEnrollment(CourseEnrollmentsVM courseEnrollmentsVM)
        {
            try
            {
                var validationResult = courseEnrollmentsVM.Validation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in Updating Employee course Enrollment: Incorrect validation");
                    TempData["UpdateEmployeeCourseEnrollmentFail"] = "Employee Course Enrollment update failed. Please ensure you enter all required details." +
                        "if the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Employees");
                }
                await _unitOfWork.CoursesEnrollmentsRepository.UpdateEmployeeCourseEnrollment(courseEnrollmentsVM);
                await _unitOfWork.SaveChangesAsync();
                TempData["UpdateEmployeeCourseEnrollmentSuccess"] = "Employee Course Enrollment successfully updated";
                return RedirectToAction(nameof(EmployeeDetailsView), "Employees", new { employeeId = courseEnrollmentsVM.EmployeeId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Updating Employee course Enrollment");
                TempData["UpdateEmployeeCourseEnrollmentFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Employees");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadEmployeeCourseEnrollmentsToExcel(int employeeId)
        {
            try
            {
                var employeeExcelFile = await _unitOfWork.CoursesEnrollmentsRepository.GetAllEmployeeCourseEnrollmentsToExcel(employeeId);
                var fileName = employeeExcelFile.FileName;
                var fileType = employeeExcelFile.FileType;
                var fileContent = employeeExcelFile.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Downloading all employees to excel");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

    }
}
