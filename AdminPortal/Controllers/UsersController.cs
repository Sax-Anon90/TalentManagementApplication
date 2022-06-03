using AdminPortal.Common.Models.UserViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using AdminPortal.Data.Data;
using AdminPortal.UI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminPortal.UI.Controllers
{
    [Authorize(Roles = Roles.SuperAdmin)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUnitOfWork _unitOfWork, ILogger<UsersController> _logger)
        {
            this._unitOfWork = _unitOfWork;
            this._logger = _logger;
        }
        // GET: UsersController
        public async Task<IActionResult> Index()
        {
            try
            {
                var allRoles = await _unitOfWork.RoleRepository.GetAllRoles();
                var usersViewModel = new UsersViewModel();
                {
                    usersViewModel.AllUsersModel = await _unitOfWork.UserRepository.GetAllUsers();

                }
                return View("Index", usersViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to get all Users data");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                    "We apologize for the inconvenience";
                return PartialView("_Error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewUser(UsersViewModel usersViewModel)
        {
            try
            {
                var validationResult = usersViewModel.userCreateModel.Validation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in trying to add new User: Incorrect Validation");
                    TempData["AddNewUserFailed"] = "User not added. Please ensure you enter all required details. if the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Users");
                }
                await _unitOfWork.UserRepository.AddNewUser(usersViewModel);
                await _unitOfWork.SaveChangesAsync();
                TempData["AddNewUserSuccess"] = "New User Successfully Added";
                return RedirectToAction(nameof(Index), "Users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to add new User");
                TempData["AddNewUserFailed"] = "Something went wrong. Please try again, if the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Users");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AssignRolesToUserView(int userId)
        {
            try
            {
                var rolesToAssign = await _unitOfWork.UserRolesRepository.GetRolesNotAssignedToUser(userId);
                if (rolesToAssign == null)
                {
                    TempData["AllRolesAssigned"] = "This employee has been assigned to all roles";
                }
                else
                {
                    rolesToAssign.Userid = userId;
                }
                return View("AssignRolesToUserView", rolesToAssign);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Erorr in Assign Roles to User View");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRolesToUser(int userId, UserRolesViewModel userRolesViewModel)
        {
            try
            {
                var result = await _unitOfWork.UserRolesRepository.AssignRolesToUser(userId, userRolesViewModel);
                if (result is false)
                {
                    _logger.LogError("Erorr in Assigning roles to Users: No roles were selected");
                    TempData["AssignRolesToUsersFail"] = "Roles not assigned to user. Please ensure you select a role." +
                        "If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Users");
                }
                await _unitOfWork.SaveChangesAsync();
                TempData["AssignRolesToUserSuccess"] = "User successfully assigned to roles";
                return RedirectToAction(nameof(Index), "Users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Assigning roles to users:");
                TempData["EnrolEmployeeToCourseFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Users");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserDetailsView(int userId)
        {
            try
            {
                var userAndRolesDetails = new UserAndRolesViewModel
                {
                    user = await _unitOfWork.UserRepository.GetUserDetails(userId),
                    userRoles = await _unitOfWork.UserRolesRepository.GetAllUserRoles(userId)
                };
                return View("UserDetailsView", userAndRolesDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in trying to get all user details data");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                    "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserRole(int userRoleId, int userId)
        {
            try
            {
                await _unitOfWork.UserRolesRepository.DeleteUserRole(userRoleId);
                await _unitOfWork.SaveChangesAsync();
                TempData["UserRoleDeleteSuccess"] = "User role successfully deleted";
                return RedirectToAction(nameof(UserDetailsView), "Users", new { userId = userId });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error in Deleting User Role");
                TempData["UserRoleDeleteFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(UserDetailsView), "Users", new { userId = userId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUserPopUpView(int userId)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserDetails(userId);
                return PartialView("_UpdateUserPopUpView", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in User Update Pop View");
                TempData["ErrorMessage"] = "Something went wrong with the data request. Please contact the systems Administrator." +
                  "We apologize for the inconvenience";
                return PartialView("_Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserViewModel userViewModel)
        {
            try
            {
                var validationResult = userViewModel.Validation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in User Update: Incorrect Validation");
                    TempData["UpdateUserFail"] = "User Update Failed. Please ensure that you enter all required details. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Users");
                }
                await _unitOfWork.UserRepository.UpdateUser(userViewModel);
                await _unitOfWork.SaveChangesAsync();
                TempData["UserUpdateSuccess"] = "User successfully updated";
                return RedirectToAction(nameof(Index), "Users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in User Update");
                TempData["UpdateEmployeeFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Users");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteUser(userId);
                await _unitOfWork.SaveChangesAsync();
                TempData["UserDeleteSuccess"] = "User successfully deleted";
                return RedirectToAction(nameof(Index), "Users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Deleting User Role");
                TempData["UserDeleteFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(Index), "Users");
            }
        }
    }
}
