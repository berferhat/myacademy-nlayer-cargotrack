using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class AuditLogController(IAuditLogService _auditLogService) : Controller
    {
        public async Task<IActionResult> Index(string? entityName, string? actionType, DateTime? fromDate, DateTime? toDate)
        {
            var logs = await _auditLogService.GetFilteredAsync(entityName, actionType, fromDate, toDate);
            
            ViewBag.EntityName = entityName;
            ViewBag.ActionType = actionType;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(logs);
        }
    }
}
