using CargoTrack.Business.Services.Dashboards;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.User.Controllers
{
    [Area(Area.User)]
    [Authorize(Roles = Roles.User)]
    public class DashboardController(
        IDashboardService _dashboardService,
        UserManager<AppUser> _userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var userIdValue = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdValue))
                return Challenge();

            var model = await _dashboardService.GetUserDashboardAsync(Guid.Parse(userIdValue));
            return View(model);
        }
    }
}