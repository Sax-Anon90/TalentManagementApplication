#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdminPortal.Common.Models;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Common.Models.CoursesViewModels;
using Microsoft.AspNetCore.Authorization;
using AdminPortal.UI.Constants;

namespace AdminPortal.UI.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(IUnitOfWork _unitOfWork, ILogger<CoursesController> _logger)
        {
            this._unitOfWork = _unitOfWork;
            this._logger = _logger;
        }

        [Authorize(Roles = Roles.Viewer + "," + Roles.HRAdmin + "," + Roles.SuperAdmin)]
        public async Task<IActionResult> Index()
        {
            try
            {
                var courseCategories = await _unitOfWork.CourseCategoriesRepository.GetAllCourseCategories();
                var CourseAndCourseCategoryModel = new CoursesAndCourseCategoriesVM()
                {
                    CoursesViewModel = await _unitOfWork.CoursesRepository.GetAllCourses(),
                    CourseCategoriesViewModel = await _unitOfWork.CourseCategoriesRepository.GetAllCourseCategories(),
                    CourseCreateViewModel = new CourseCreateViewModel
                    {
                        CourseCategories = new SelectList(courseCategories, "Id", "CategoryName")
                    }
                };
                return View(CourseAndCourseCategoryModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to get all course data");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                    "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCourse(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM)
        {
            try
            {
                var validationResult = coursesAndCourseCategoriesVM.CourseCreateViewModel.Validation();
                if (validationResult == false)
                {
                    _logger.LogError("Error in trying to add new course: Incorrect Validation");
                    TempData["AddNewCourseFailed"] = "Course not added. Please ensure you enter all required details. if the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Courses");
                }
                await _unitOfWork.CoursesRepository.AddNewCourse(coursesAndCourseCategoriesVM);
                await _unitOfWork.SaveChangesAsync();
                TempData["AddNewCourseSuccess"] = "New Course Successfully Added";
                return RedirectToAction(nameof(Index), "Courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to add new course");
                TempData["AddNewCourseFailed"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCourseFileAttachment(CoursesAndCourseCategoriesVM courseFileAttachment, int courseId)
        {
            try
            {
                var result = await _unitOfWork.CourseFileAttachmentsRepository.UploadCourseFileAttachment(courseFileAttachment, courseId);
                if (result == false)
                {
                    _logger.LogError("Error in trying to upload course file: Invalid file");
                    TempData["FileUploadFail"] = "Course File Upload failed. Please ensure that you have entered a valid file. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Courses");
                }
                await _unitOfWork.SaveChangesAsync();
                TempData["FileUploadSuccess"] = "Course File Uploaded successfully";
                return RedirectToAction(nameof(Index), "Courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to upload course file");
                TempData["FileUploadFail"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
        }

        [Authorize(Roles = Roles.Viewer + "," + Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> CourseDetailsView(int? courseId)
        {
            try
            {
                var CourseDetailsAndCourseFiles = new CourseDetailsAndCourseFilesVM
                {
                    CourseDetails = await _unitOfWork.CoursesRepository.GetCourseDetails(courseId),
                    CourseFileAttachments = await _unitOfWork.CourseFileAttachmentsRepository.GetAllCourseFileAttachmentsById(courseId),
                    EmployeesInCourse = await _unitOfWork.CoursesEnrollmentsRepository.GetAllEmployeesEnrolledInCourse(courseId),
                    EmployeesCompletedCourses = await _unitOfWork.CoursesEnrollmentsRepository.GetAllCoursesCompletedByEmployee(courseId),
                    EmployeesInProcessCourses = await _unitOfWork.CoursesEnrollmentsRepository.GetAllCoursesInProcessByEmployee(courseId)

                };
                return View("CourseDetailsView", CourseDetailsAndCourseFiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to get all course details data");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                    "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadCourseFileAttachment(int Id)
        {
            try
            {
                var courseFileAttachment = await _unitOfWork.CourseFileAttachmentsRepository.GetCourseFileAttachment(Id);
                var fileName = courseFileAttachment.FileName;
                var fileType = courseFileAttachment.FileType;
                var fileContent = courseFileAttachment.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to dowmload course file attachment");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                   "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<PartialViewResult> UpdateCoursePopUpView(int? courseId)
        {
            try
            {
                var course = await _unitOfWork.CoursesRepository.GetCourseDetails(courseId);
                var courseCategories = await _unitOfWork.CourseCategoriesRepository.GetAllCourseCategories();
                course.CourseCategories = new SelectList(courseCategories, "Id", "CategoryName");
                return PartialView("_UpdateCoursePopUpViewModal", course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Update Course pop up view");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                    "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCourse(CoursesVM courseUpdateModal)
        {
            try
            {
                var validationResult = courseUpdateModal.Validation();
                if (validationResult == false)
                {
                    _logger.LogError("Error in Update Course pop up view");
                    TempData["UpdateCourseFail"] = "Course Update Failed. Please ensure that you enter all required details. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Courses");
                }
                await _unitOfWork.CoursesRepository.UpdateCourse(courseUpdateModal);
                await _unitOfWork.SaveChangesAsync();
                TempData["CourseUpdateSuccess"] = "Course successfully updated";
                return RedirectToAction(nameof(Index), "Courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to Update the Course");
                TempData["UpdateCourseFail"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourseFileAttachment(int courseFileAttachmentId, int courseId)
        {
            try
            {
                await _unitOfWork.CourseFileAttachmentsRepository.DeleteCourseFileAttachment(courseFileAttachmentId);
                await _unitOfWork.SaveChangesAsync();
                TempData["CourseFileDeleteSuccess"] = "Course File Attachment successfully deleted";
                return RedirectToAction(nameof(CourseDetailsView), "Courses", new { courseId = courseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to delete the course file attachment");
                TempData["CourseFileDeleteFail"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(CourseDetailsView), "Courses", new { courseId = courseId });
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            try
            {
                await _unitOfWork.CoursesRepository.DeleteCourse(Id);
                await _unitOfWork.SaveChangesAsync();
                TempData["CourseDeleteSuccess"] = "Course Successfully Deleted";
                return RedirectToAction(nameof(Index), "Courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to delete the course");
                TempData["CourseDeleteFail"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCourseCategory(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM)
        {
            try
            {
                var validationResult = coursesAndCourseCategoriesVM.courseCategoryCreateVM.validation();
                if (validationResult == false)
                {
                    _logger.LogError("Error in trying to add a new course category: Incorrect Validation");
                    TempData["CourseCategoryFail"] = "Course Category not added. Please ensure you have entered all required details. If the problem persists contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Courses");
                }
                await _unitOfWork.CourseCategoriesRepository.AddNewCourseCategory(coursesAndCourseCategoriesVM);
                await _unitOfWork.SaveChangesAsync();
                TempData["CourseCategorySucess"] = "Course Category added successfully.";
                return RedirectToAction(nameof(Index), "Courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to add a new course category");
                TempData["CourseCategoryFail"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
        }

        [Authorize(Roles = Roles.Viewer + "," + Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> CourseCategoryDetailsView(int courseCategoryId)
        {
            try
            {
                var courseCategory = await _unitOfWork.CourseCategoriesRepository.GetCourseCategoryDetails(courseCategoryId);
                return View("CourseCategoryDetailsView", courseCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in the course category details view");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                   "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> UpdateCourseCategoryPopUpView(int courseCategoryId)
        {
            try
            {
                var courseCategory = await _unitOfWork.CourseCategoriesRepository.GetCourseCategoryDetails(courseCategoryId);
                return PartialView("_UpdateCourseCategoryPopUpView", courseCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in the update course category update pop up view");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                   "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCourseCategory(CourseCategoryVM courseCategoryVM)
        {
            try
            {
                var validationResult = courseCategoryVM.validation();
                if (validationResult == false)
                {
                    _logger.LogError("Error in trying to update course category: Incorrect validation");
                    TempData["CourseCategoryUpdateFail"] = "Course Category not updated. Please ensure that you have entered all required details. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Courses");
                }
                await _unitOfWork.CourseCategoriesRepository.UpdateCourseCategory(courseCategoryVM);
                await _unitOfWork.SaveChangesAsync();
                TempData["CourseCategoryUpdateSucess"] = "Course Category updated successfully.";
                return RedirectToAction(nameof(Index), "Courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to update course category");
                TempData["CourseCategoryUpdateFail"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourseCategory(int courseCategoryId)
        {
            try
            {
                await _unitOfWork.CourseCategoriesRepository.DeleteCourseCategory(courseCategoryId);
                await _unitOfWork.SaveChangesAsync();
                TempData["CourseCategoryDeleteSuccess"] = "Course Category Successfully Deleted";
                return RedirectToAction(nameof(Index), "Courses");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to delete a course category");
                TempData["CourseCategoryDeleteFail"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadEmployeesEnrolledInCourseToExcel(int CourseId)
        {
            try
            {
                var employeesEnrolledInCourse = await _unitOfWork.CoursesEnrollmentsRepository.GetAllEmployeesEnrolledInCourseToExcel(CourseId);
                var fileName = employeesEnrolledInCourse.FileName;
                var fileType = employeesEnrolledInCourse.FileType;
                var fileContent = employeesEnrolledInCourse.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to donwload employees enrolled in a course to excel");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                   "We apologize for the inconvenience";
                return PartialView("_Error");
            }

        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadAllCoursesToExcel()
        {
            try
            {
                var courseExcelFile = await _unitOfWork.CoursesRepository.GetAllCoursesDataToExcelFile();
                var fileName = courseExcelFile.FileName;
                var fileType = courseExcelFile.FileType;
                var fileContent = courseExcelFile.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to download all courses to excel");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                   "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadEmployeesWhoCompletedCourseToExcel(int courseId)
        {
            try
            {
                var employeesExcelFile = await _unitOfWork.CoursesEnrollmentsRepository.GetAllEmployeesWhoCompletedCourseToExcel(courseId);
                var fileName = employeesExcelFile.FileName;
                var fileType = employeesExcelFile.FileType;
                var fileContent = employeesExcelFile.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to download employees who completed a course to excel");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadEmployeesInProccessToExcel(int courseId)
        {
            try
            {
                var employeesExcelFile = await _unitOfWork.CoursesEnrollmentsRepository.GetAllEmployeesInProccessToExcel(courseId);
                var fileName = employeesExcelFile.FileName;
                var fileType = employeesExcelFile.FileType;
                var fileContent = employeesExcelFile.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to download emplpyees In proccess to excel");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [Authorize(Roles = Roles.HRAdmin + "," + Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> DownloadCoursesInCourseCategoryToExcel(int courseCategoryId)
        {
            try
            {
                var coursesInCourseCategoryExcelFile = await _unitOfWork.CourseCategoriesRepository.GetAllCoursesInCourseCategoryToExcel(courseCategoryId);
                var fileName = coursesInCourseCategoryExcelFile.FileName;
                var fileType = coursesInCourseCategoryExcelFile.FileType;
                var fileContent = coursesInCourseCategoryExcelFile.FileContent;
                return File(fileContent, fileType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to download Courses In Course Categpry to excel");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }
    }
}
