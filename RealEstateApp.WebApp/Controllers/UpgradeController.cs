using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Upgrade;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UpgradeController : Controller
    {
        private readonly IUpgradeService _upgradeService;

        public UpgradeController(IUpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
        }

        public async Task<IActionResult> Index() => View(await _upgradeService.GetAllAsync());
        public IActionResult Create() => View("SaveUpgrade", new SaveUpgradeViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(SaveUpgradeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveUpgrade", vm);
            }
            await _upgradeService.CreateAsync(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int Id)
        {
            SaveUpgradeViewModel vm = await _upgradeService.GetByIdSaveViewModelAsync(Id);
            return View("SaveUpgrade", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUpgradeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveUpgrade", vm);
            }
            await _upgradeService.UpdateAsync(vm, (Int32)vm.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            await _upgradeService.DeleteAsync(Id);
            return RedirectToAction("Index");
        }
    }
}
