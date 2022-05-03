#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdminPortal.Data;
using AdminPortal.Common.Models;
using AdminPortal.CoreBusiness.Repositories.Contracts;

namespace AdminPortal.UI.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoursesController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _unitOfWork.CoursesRepository.GetAllCourses();
            var courseCategories = await _unitOfWork.CourseCategoriesRepository.GetAllCourseCategories();
            var CourseAndCourseCategoryModel = new CoursesAndCourseCategoriesVM()
            {
                CoursesViewModel = courses,
                CourseCategoriesViewModel = courseCategories,
                CourseCreateViewModel = new CourseCreateViewModel
                {
                    CourseCategories = new SelectList(courseCategories, "Id", "CategoryName")
                }
            };
            return View(CourseAndCourseCategoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCourse(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM)
        {
            var validationResult = coursesAndCourseCategoriesVM.CourseCreateViewModel.Validation();
            if (validationResult == false)
            {
                TempData["AddNewCourseFailed"] = "Course not added. Please ensure you enter all required details. if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
            await _unitOfWork.CoursesRepository.AddNewCourse(coursesAndCourseCategoriesVM);
            TempData["AddNewCourseSuccess"] = "New Course Successfully Added";
            return RedirectToAction(nameof(Index), "Courses");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCourseFileAttachment(CoursesAndCourseCategoriesVM courseFileAttachment, int courseId)
        {
            var result = await _unitOfWork.CourseFileAttachmentsRepository.UploadCourseFileAttachment(courseFileAttachment, courseId);
            if (result == false)
            {
                TempData["FileUploadFail"] = "Course File Upload failed. Please ensure that you have entered a valid file. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
            TempData["FileUploadSuccess"] = "Course File Uploaded successfully";
            return RedirectToAction(nameof(Index), "Courses");
        }

        [HttpGet]
        public async Task<PartialViewResult> CourseDetailsPopUpView(int? courseId)
        {
            var courseDetails = await _unitOfWork.CoursesRepository.GetCourseDetails(courseId);
            var courseFileAttachments = await _unitOfWork.CourseFileAttachmentsRepository.GetAllCourseFileAttachmentsById(courseId);
            var CourseDetailsAndCourseFiles = new CourseDetailsAndCourseFilesVM
            {
                CourseDetails = courseDetails,
                CourseFileAttachments = courseFileAttachments
            };
            return PartialView("_CourseDetailsPopUpViewModal", CourseDetailsAndCourseFiles);
        }
        public async Task<IActionResult> DownloadCourseFileAttachment(int Id)
        {
            var courseFileAttachment = await _unitOfWork.CourseFileAttachmentsRepository.GetCourseFileAttachment(Id);
            var fileName = courseFileAttachment.FileName;
            var fileType = courseFileAttachment.FileType;
            var fileContent = courseFileAttachment.FileContent;
            return File(fileContent, fileType, fileName);
        }

        [HttpGet]
        public async Task<PartialViewResult> UpdateCoursePopUpView(int? courseId)
        {
            var course = await _unitOfWork.CoursesRepository.GetCourseDetails(courseId);
            var courseCategories = await _unitOfWork.CourseCategoriesRepository.GetAllCourseCategories();
            course.CourseCategories = new SelectList(courseCategories, "Id", "CategoryName");
            return PartialView("_UpdateCoursePopUpViewModal", course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCourse(CoursesVM courseUpdateModal)
        {
            var validationResult = courseUpdateModal.Validation();
            if (validationResult == false)
            {
                TempData["UpdateCourseFail"] = "Course Update Failed. Please ensure that you enter all required details. If the problem persists, contact the systems administrator";
                RedirectToAction(nameof(Index), "Courses");
            }
            await _unitOfWork.CoursesRepository.UpdateCourse(courseUpdateModal);
            TempData["CourseUpdateSuccess"] = "Course successfully updated";
            return RedirectToAction(nameof(Index), "Courses");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourseFileAttachment(int courseFileAttachmentId)
        {
            await _unitOfWork.CourseFileAttachmentsRepository.DeleteCourseFileAttachment(courseFileAttachmentId);
            TempData["CourseFileDeleteSuccess"] = "Course File Attachment successfully deleted";
            return RedirectToAction(nameof(Index), "Courses");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            await _unitOfWork.CoursesRepository.DeleteCourse(Id);
            TempData["CourseDeleteSuccess"] = "Course Successfully Deleted";
            return RedirectToAction(nameof(Index), "Courses");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewCourseCategory(CoursesAndCourseCategoriesVM coursesAndCourseCategoriesVM)
        {
            var validationResult = coursesAndCourseCategoriesVM.courseCategoryCreateVM.validation();
            if (validationResult == false)
            {
                TempData["CourseCategoryFail"] = "Course Category not added. Please ensure you have added all required details. If the problem persists contact the systems administrator";
                return RedirectToAction(nameof(Index), "Courses");
            }
            await _unitOfWork.CourseCategoriesRepository.AddNewCourseCategory(coursesAndCourseCategoriesVM);
            TempData["CourseCategorySucess"] = "Course Category added successfully.";
            return RedirectToAction(nameof(Index), "Courses");
        }


    }
}
