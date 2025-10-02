using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Account;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeveloperController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRealEstatePropertyService _realEstatePropertyService;

        public DeveloperController(IUserService userService, IRealEstatePropertyService realEstatePropertyService)
        {
            _userService = userService;
            _realEstatePropertyService = realEstatePropertyService;
        }

        public async Task<IActionResult> Index()
        {
            List<UserViewModel> developers = await _userService.GetAllByRoleViewModel(nameof(UserRoles.Developer));
            foreach (UserViewModel user in developers)
            {
                user.PropertyCount = await _realEstatePropertyService.GetTotalPropertiesByAgent(user.Id);
            }
            return View(developers);
        }

        public IActionResult Create() => View("SaveDeveloper", new SaveUserViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveDeveloper", vm);
            }
            var origin = Request.Headers["origin"];
            UserRegisterResponse response = await _userService.RegisterUserAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveDeveloper", vm);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string Id)
        {
            SaveUserViewModel developer = await _userService.GetByIdSaveViewModelAsync(Id);
            return View("SaveDeveloper", developer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            vm.Role = nameof(UserRoles.Developer);
            var origin = Request.Headers["origin"];
            UserEditResponse response = await _userService.EditUserAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View("SaveDeveloper", vm);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivateUser(string Id)
        {
            await _userService.ActivateUser(Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeactivateUser(string Id)
        {
            await _userService.DeactivateUser(Id);
            return RedirectToAction("Index");
        }
    }
}
