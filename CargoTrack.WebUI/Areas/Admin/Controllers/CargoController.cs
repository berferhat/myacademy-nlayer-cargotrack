using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class CargoController(ICargoService _cargoService, IBranchService _branchService) : Controller
    {
        public async Task<IActionResult> Index(string? search, CargoStatus? status, Guid? branchId, int page = 1)
        {
            const int pageSize = 10;

            var cargos = await _cargoService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
                cargos = cargos.Where(c => c.TrackCode.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (status.HasValue)
                cargos = cargos.Where(c => c.CargoStatus == status.Value).ToList();

            if (branchId.HasValue)
                cargos = cargos.Where(c => c.OriginBranchId == branchId.Value || c.DestinationBranchId == branchId.Value).ToList();

            var totalCount = cargos.Count;
            var items = cargos
                .OrderByDescending(c => c.ShipmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var branches = await _branchService.GetAllAsync();
            ViewBag.branches = branches.Select(b => new SelectListItem
            {
                Text = b.Name,
                Value = b.Id.ToString(),
                Selected = branchId == b.Id
            }).ToList();

            ViewBag.statuses = Enum.GetValues<CargoStatus>()
                .Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = ((int)s).ToString(),
                    Selected = status == s
                }).ToList();

            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View(items);
        }
    }
}
