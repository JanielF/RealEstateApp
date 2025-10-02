using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalesTypeController : Controller
    {
        private readonly ITypeOfSaleService _typeOfSaleService;

        public SalesTypeController(ITypeOfSaleService typeOfSaleService)
        {
            _typeOfSaleService = typeOfSaleService;
        }

        public async Task<IActionResult> Index() => View(await _typeOfSaleService.GetAllWithIncludeAsync(new List<string> { "Properties" }));

        public IActionResult Create() => View("SaveSaleType", new SaveTypeOfSaleViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(SaveTypeOfSaleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveSaleType", vm);
            }
            await _typeOfSaleService.CreateAsync(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int Id) 
        {
            SaveTypeOfSaleViewModel vm = await _typeOfSaleService.GetByIdSaveViewModelAsync(Id);
            return View("SaveSaleType", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveTypeOfSaleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveSaleType", vm);
            }
            await _typeOfSaleService.UpdateAsync(vm, (Int32)vm.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int Id)
        {
            await _typeOfSaleService.DeleteAsync(Id);
            return RedirectToAction("Index");
        }
    }
}
