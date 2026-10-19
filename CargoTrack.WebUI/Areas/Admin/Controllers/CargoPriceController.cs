using CargoTrack.Business.Services.CargoPrices;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class CargoPriceController(ICargoPriceService _cargoPriceService) : Controller
    {
        private void LoadLookups()
        {
            ViewBag.cargoTypes = Enum.GetValues<CargoType>()
                .Select(t => new SelectListItem
                {
                    Text = t.ToString(),
                    Value = ((int)t).ToString()
                }).ToList();
        }

        public async Task<IActionResult> Index()
        {
            var list = await _cargoPriceService.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadLookups();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCargoPriceDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadLookups();
                return View(dto);
            }

            await _cargoPriceService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            LoadLookups();
            var item = await _cargoPriceService.GetByIdAsync(id);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCargoPriceDto dto)
        {
            if (!ModelState.IsValid)
            {
                LoadLookups();
                return View(dto);
            }

            await _cargoPriceService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _cargoPriceService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
