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
        private readonly SaxTalentManagementContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(SaxTalentManagementContext context, IUnitOfWork _unitOfWork)
        {
            _context = context;
            this._unitOfWork = _unitOfWork;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var EmployeesAndEmployeeTrainingModel = new EmployeeAndEmployeeTrainingVM()
            {
                EmployeesViewModel = await _unitOfWork.EmployeeRepository.GetAllEmployees()
            };
            return View(EmployeesAndEmployeeTrainingModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM)
        {
            var validationResult = employeeAndEmployeeTrainingVM.EmployeeCreateViewModel.Validation();
            if(validationResult is false)
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
        public async Task<IActionResult> UploadEmployeeFileAttachment(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM, int employeeId)
        {
            var result = await _unitOfWork.EmployeeFileAttachmentsRepository.UploadEmployeeFileAttachment(employeeAndEmployeeTrainingVM, employeeId);
            await _unitOfWork.SaveChangesAsync();
            if(result == false)
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
        public async Task<IActionResult> SearchEmployee(EmployeeAndEmployeeTrainingVM employeeAndEmployeeTrainingVM)
        {
            var employeeNo = employeeAndEmployeeTrainingVM.EmployeeSearchResult.EmployeeNo.ToString();
            var searchResult = await _unitOfWork.EmployeeRepository.SearchEmployeeByEmployeeNo(employeeNo);
            if(searchResult == null)
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
            coursesToEnrol.EmployeeId = employeeId;
            if(coursesToEnrol == null)
            {
                TempData["AllCoursesEnrolled"] = "The employee has enrolled in all courses";
            }
            return View("EnrolEmployeeToCourseView", coursesToEnrol);
        }

        [HttpPost]
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

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeNo,FirstName,LastName,Title,Department,Location,StartDate,EndDate,BirthDate,Age,Race,Gender,Email,Region,ReportsToManager,PositionTitle,University,HighestQualification")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeNo,FirstName,LastName,Title,Department,Location,StartDate,EndDate,BirthDate,Age,Race,Gender,Email,Region,ReportsToManager,PositionTitle,University,HighestQualification")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
