using AdminPortal.Common.Models.AuthenicationViewModels;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.UI.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IUnitOfWork _unitOfWork, ILogger<AuthenticationController> _logger)
        {
            this._unitOfWork = _unitOfWork;
            this._logger = _logger;
        }
        public async Task<IActionResult> Index(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View("Index", loginViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> RegistrationView()
        {
            var registrationModel = new RegistrationViewModel();
            return View("RegistrationView", registrationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIfUserExists(RegistrationViewModel registrationViewModel)
        {
            try
            {
                var validationResult = registrationViewModel.Validation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in Registration in trying to check if user exists: Incorrect validation");
                    TempData["UserExistsFail"] = "Something went wrong. Please ensure that you have entered all required details. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(RegistrationView));
                }
                var userExists = await _unitOfWork.UserRepository.CheckIfUserExists(registrationViewModel);
                if (userExists is false)
                {
                    TempData["UserExistsFail"] = "Something went wrong. Please ensure that you have been added" +
                       " as a user for this system by the systems administrator. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(RegistrationView));
                }
                else
                {

                    return RedirectToAction(nameof(SetUpPasswordView), new
                    {
                        employeeNo = registrationViewModel.employeeNo,
                        email = registrationViewModel.Email
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Registration in trying to check if user exists");
                TempData["UserExistsFail"] = "Something went wrong. Please try again. If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(RegistrationView));
            }
        }

        [HttpGet]
        public async Task<IActionResult> SetUpPasswordView(string employeeNo, string email)
        {
            var setUpPasswordModel = new SetUpPasswordViewModel()
            {
                Email = email,
                employeeNo = employeeNo
            };
            return View("SetUpPasswordView", setUpPasswordModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetUpPasswordForUser(SetUpPasswordViewModel setUpPasswordViewModel)
        {
            try
            {
                var validationResult = setUpPasswordViewModel.Validation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in Registration in trying to set up Password for User: Incorrect validation");
                    TempData["SetUpPasswordForUserFail"] = "Something went wrong. Please ensure that you have entered all required details and that the " +
                        "password you entered matches with the password retyped. If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(SetUpPasswordView), new
                    {
                        employeeNo = setUpPasswordViewModel.employeeNo,
                        email = setUpPasswordViewModel.Email
                    });
                }
                var passwordSetUpResult = await _unitOfWork.UserRepository.SetUpPasswordForUser(setUpPasswordViewModel);
                if (passwordSetUpResult is false)
                {
                    _logger.LogError("Error in Registration in trying to set up Password for User: Either user not found or something went wrong with the passwordhash service");
                    TempData["SetUpPasswordForUserFail"] = "Something went wrong. Please try again, If the problem persists, contact the systems administrator";
                    return RedirectToAction(nameof(SetUpPasswordView), new
                    {
                        employeeNo = setUpPasswordViewModel.employeeNo,
                        email = setUpPasswordViewModel.Email
                    });
                }
                else
                {
                    await _unitOfWork.SaveChangesAsync();
                    TempData["SetUpPasswordSuccess"] = "Password successfully set up! Now you can Login";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Registration in trying to set up Password for User");
                TempData["SetUpPasswordForUserFail"] = "Something went wrong. Please try again, If the problem persists, contact the systems administrator";
                return RedirectToAction(nameof(SetUpPasswordView), new
                {
                    employeeNo = setUpPasswordViewModel.employeeNo,
                    email = setUpPasswordViewModel.Email
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var validationResult = loginViewModel.Validation();
                if (validationResult is false)
                {
                    _logger.LogError("Error in trying in to Login: Incorrect Validation");
                    TempData["LoginErrorMessage"] = "Login Failed. Please ensure that you have entered all details and that your username and password is correct. If the problem persists contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Authentication", loginViewModel.ReturnUrl);
                }
                var userLoginClaimsPrincipal = await _unitOfWork.AuthenticationRepository.Login(loginViewModel);
                if (userLoginClaimsPrincipal is null)
                {
                    _logger.LogError("Error in Registration in to Login: Email or Password is incorrect");
                    TempData["LoginErrorMessage"] = "Login Failed. Please ensure that you have entered all details and that your username and password is correct. If the problem persists contact the systems administrator";
                    return RedirectToAction(nameof(Index), "Authentication", loginViewModel.ReturnUrl);
                }
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userLoginClaimsPrincipal);
                return LocalRedirect(loginViewModel.ReturnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Registration in to Login");
                TempData["LoginErrorMessage"] = "Login Failed. Please ensure that you have entered all details and that your username and password is correct. If the problem persists contact the systems administrator";
                return RedirectToAction(nameof(Index), new
                {
                    ReturnUrl = loginViewModel.ReturnUrl
                });
            }
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("~/");
        }
    }
}
