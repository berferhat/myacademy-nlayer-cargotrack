using CargoTrack.Business.Services.Dashboards;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class DashboardController(IDashboardService _dashboardService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await _dashboardService.GetAdminDashboardAsync();
            return View(model);
        }
    }
}
