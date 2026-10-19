using CargoTrack.Business.Services.Cargos;
using CargoTrack.DTO.DTOs.CargoDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.WebUI.Areas.User.Controllers
{
    [Area(Area.User)]
    [Authorize(Roles = Roles.User)]
    public class CargoController(ICargoService _cargoService,
                                 UserManager<AppUser> _userManager) : Controller
    {
        private Guid GetCurrentUserId()
        {
            var userIdValue = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userIdValue))
                throw new UnauthorizedAccessException();

            return Guid.Parse(userIdValue);
        }

        private async Task<(List<ResultCargoDto> All, Guid UserId)> LoadUserCargosAsync()
        {
            var userId = GetCurrentUserId();
            var all = await _cargoService.GetForUserAsync(userId);
            return (all, userId);
        }

        private void SetListViewBag(List<ResultCargoDto> all, Guid userId, string activeTab)
        {
            var active = all.Where(c => c.CargoStatus != CargoStatus.Delivered
                                     && c.CargoStatus != CargoStatus.ReturnedToSender).ToList();

            ViewBag.ActiveTab = activeTab;
            ViewBag.ActiveCount = active.Count;
            ViewBag.IncomingCount = all.Count(c => c.ReceiverId == userId);
            ViewBag.OutgoingCount = all.Count(c => c.SenderId == userId);
            ViewBag.HistoryCount = all.Count(c => c.CargoStatus == CargoStatus.Delivered
                                               || c.CargoStatus == CargoStatus.ReturnedToSender);
            ViewBag.OutForDeliveryCount = all.Count(c => c.CargoStatus == CargoStatus.OutForDelivery);
            ViewBag.DeliveredCount = all.Count(c => c.CargoStatus == CargoStatus.Delivered);
            ViewBag.TotalPrice = all.Where(c => c.Price.HasValue).Sum(c => c.Price!.Value);
        }

        public async Task<IActionResult> Index()
        {
            var (all, userId) = await LoadUserCargosAsync();
            SetListViewBag(all, userId, "aktif");

            var list = all
                .Where(c => c.CargoStatus != CargoStatus.Delivered
                         && c.CargoStatus != CargoStatus.ReturnedToSender)
                .ToList();

            return View(list);
        }

        public async Task<IActionResult> Incoming()
        {
            var (all, userId) = await LoadUserCargosAsync();
            SetListViewBag(all, userId, "gelen");
            return View("Index", all.Where(c => c.ReceiverId == userId).ToList());
        }

        public async Task<IActionResult> Outgoing()
        {
            var (all, userId) = await LoadUserCargosAsync();
            SetListViewBag(all, userId, "giden");
            return View("Index", all.Where(c => c.SenderId == userId).ToList());
        }

        public async Task<IActionResult> History()
        {
            var (all, userId) = await LoadUserCargosAsync();
            SetListViewBag(all, userId, "gecmis");

            var list = all
                .Where(c => c.CargoStatus == CargoStatus.Delivered
                         || c.CargoStatus == CargoStatus.ReturnedToSender)
                .ToList();

            return View("Index", list);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var details = await _cargoService.GetDetailsForUserAsync(id, GetCurrentUserId());
                return View(details);
            }
            catch (ValidationException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
