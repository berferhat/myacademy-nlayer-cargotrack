using CargoTrack.Business.Services.Cities;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.TransferCenterDtos;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class TransferCenterController(ITransferCenterService _transferCenterService,
                                          ICityService _cityService) : Controller
    {
        private async Task GetCitiesAsync()
        {
            var cities = await _cityService.GetAllAsync();

            ViewBag.cities = cities.Select(city => new SelectListItem
            {
                Text = city.Name,
                Value = city.Id.ToString()
            }).ToList();
        }

        public async Task<IActionResult> Index()
        {
            var list = await _transferCenterService.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await GetCitiesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransferCenterDto transferCenterDto)
        {
            if(!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(transferCenterDto);
            }

            await _transferCenterService.CreateAsync(transferCenterDto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await GetCitiesAsync();
            var item = await _transferCenterService.GetByIdAsync(id);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTransferCenterDto dto)
        {
            if (!ModelState.IsValid)
            {
                await GetCitiesAsync();
                return View(dto);
            }

            await _transferCenterService.UpdateAsync(dto);
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _transferCenterService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
