using CargoTrack.Business.Services.Cargos;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Controllers
{
    public class DefaultController(ICargoService _cargoService) : Controller
    {
        public IActionResult Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            return View();
        }

        public async Task<IActionResult> CargoDetails(string trackCode)
        {
            var cargo = await _cargoService.GetByTrackCodeAsync(trackCode);

            if (cargo is null)
            {
                TempData["error"] = "Bu Takip Numarasına Ait Bir Kargo Bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(cargo);
        }
    }
}