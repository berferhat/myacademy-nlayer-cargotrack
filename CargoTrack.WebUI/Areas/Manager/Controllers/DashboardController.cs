using CargoTrack.Business.Services.Dashboards;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using CargoTrack.DTO.DTOs.DashboardDtos;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    [Authorize(Roles = Roles.Manager)]
    public class DashboardController(IDashboardService _dashboardService, UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var userIdValue = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userIdValue))
                return Challenge();

            try
            {
                var model = await _dashboardService.GetManagerDashboardAsync(Guid.Parse(userIdValue));
                return View(model);
            }
            catch (ValidationException ex)
            {
                TempData["error"] = ex.Message;
                return View(new ManagerDashboardDto());
            }
        }
    }
}
