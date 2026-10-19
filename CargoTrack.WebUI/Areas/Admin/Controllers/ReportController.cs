using CargoTrack.Business.Services.Reports;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class ReportController(IReportService _reportService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await _reportService.GetAdminReportAsync();
            return View(model);
        }
    }
}
