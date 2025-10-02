using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.WebApp.Middlewares;

namespace RealEstateApp.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor contextAccessor)
        {
            _userService = userService;
            _contextAccessor = contextAccessor;
        }
        
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.AuthenticateAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);

                if (userVm.Roles.Any(r => r == "Customer"))
                {
                    return RedirectToRoute(new { controller = "Home", action = "CustomerHome" });
                }
                else if (userVm.Roles.Any(r => r == "RealEstateAgent"))
                {
                    return RedirectToRoute(new { controller = "Agent", action = "AgentHome" });
                }
                else
                {
                    return RedirectToRoute(new { controller = "Administrator", action = "AdminHome" });
                }
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogOutAsync()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Create() => View("SaveUser", new SaveUserViewModel());

        [HttpPost]
        [ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View("SaveUser", vm);
            }
            var origin = Request.Headers["origin"];
            UserRegisterResponse response = await _userService.RegisterUserAsync(vm, origin);
            
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveUser", vm);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _userService.ConfirmAccountAsync(userId, token);
            return View("ConfirmEmail");
        }
    }
}
