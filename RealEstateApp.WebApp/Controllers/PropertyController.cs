using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize]
    public class PropertyController : Controller
    {
        private readonly IRealEstatePropertyService _propertyService;
        private readonly ITypeOfPropertyService _typeOfPropertyService;
        private readonly ITypeOfSaleService _typeOfSaleService;
        private readonly IUpgradeService _upgradeService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;
        private readonly IFavoritePropertyService _favoritePropertyService;

        public PropertyController(IRealEstatePropertyService propertyService, ITypeOfPropertyService typeOfPropertyService, ITypeOfSaleService typeOfSaleService, IUpgradeService upgradeService, IHttpContextAccessor contextAccessor, IUserService userService, IFavoritePropertyService favoritePropertyService)
        {
            _propertyService = propertyService;
            _typeOfPropertyService = typeOfPropertyService;
            _typeOfSaleService = typeOfSaleService;
            _upgradeService = upgradeService;
            _contextAccessor = contextAccessor;
            _userService = userService;
            _favoritePropertyService = favoritePropertyService;
        }

        public async Task<IActionResult> AgentProperty(string Id)
        {
            var properties = await _propertyService.GetByAgentAsync(Id);
            return View(properties.Count() > 0 ? properties.OrderByDescending(d => d.Created).ToList() : properties);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerProperties()
        {
            var properties = await _favoritePropertyService.GetAllPropertyByUser(_contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user").Id);
            return View(properties.Count() > 0 ? properties.OrderByDescending(d => d.Created).ToList() : properties);
        }

        [Authorize(Roles = "RealEstateAgent")]
        public async Task<IActionResult> Create()
        {
            SaveRealEstatePropertyViewModel vm = new();
            vm.UpgradeList = await _upgradeService.GetAllAsync();
            vm.TypeOfPropertyList = await _typeOfPropertyService.GetAllAsync();
            vm.TypeOfSaleList = await _typeOfSaleService.GetAllAsync();
            return View(vm);
        }

        [Authorize(Roles = "RealEstateAgent")]
        [HttpPost]
        public async Task<IActionResult> Create(SaveRealEstatePropertyViewModel vm)
        {
            var agentlogged = _contextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            var agent = await _userService.GetByIdAsync(agentlogged.Id);
            vm.AgentId = agent.Id;
            vm.AgentName = agent.LastName;

            if (!ModelState.IsValid)
            {
                vm.UpgradeList = await _upgradeService.GetAllAsync();
                vm.TypeOfPropertyList = await _typeOfPropertyService.GetAllAsync();
                vm.TypeOfSaleList = await _typeOfSaleService.GetAllAsync();
                return View(vm);
            }
            var property = await _propertyService.CreateAsync(vm);
            if (property.HasError)
            {
                vm.HasError = property.HasError;
                vm.Error = property.Error;
                vm.UpgradeList = await _upgradeService.GetAllAsync();
                vm.TypeOfPropertyList = await _typeOfPropertyService.GetAllAsync();
                vm.TypeOfSaleList = await _typeOfSaleService.GetAllAsync();
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Agent", action = "AgentPropertyMaintenance" });
        }

        [Authorize(Roles = "RealEstateAgent")]
        public async Task<IActionResult> Edit(int Id)
        {
            SaveRealEstatePropertyViewModel vm = await _propertyService.GetByIdSaveViewModelAsync(Id);
            vm.UpgradeList = await _upgradeService.GetAllAsync();
            vm.TypeOfPropertyList = await _typeOfPropertyService.GetAllAsync();
            vm.TypeOfSaleList = await _typeOfSaleService.GetAllAsync();
            return View(vm);
        }

        [Authorize(Roles = "RealEstateAgent")]
        [HttpPost]
        public async Task<IActionResult> Edit(SaveRealEstatePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.UpgradeList = await _upgradeService.GetAllAsync();
                vm.TypeOfPropertyList = await _typeOfPropertyService.GetAllAsync();
                vm.TypeOfSaleList = await _typeOfSaleService.GetAllAsync();
                return View(vm);
            }
            
            SaveRealEstatePropertyViewModel property = await _propertyService.GetByIdSaveViewModelAsync((Int32)vm.Id);
            await _propertyService.UpdateAsync(vm, (Int32)property.Id);
            return RedirectToRoute(new { controller = "Agent", action = "AgentPropertyMaintenance" });
        }

        [Authorize(Roles = "RealEstateAgent")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _propertyService.DeleteAsync(Id);
            return RedirectToRoute(new { controller = "Agent", action = "AgentPropertyMaintenance" });
        }

    }
}
