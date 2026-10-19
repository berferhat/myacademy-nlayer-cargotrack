using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DTO.DTOs.CargoDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.WebUI.Areas.Manager.Controllers
{
    [Area(Area.Manager)]
    [Authorize(Roles = Roles.Manager)]
    public class CargoController(ICargoService _cargoService,
                                 IBranchService _branchService,
                                 UserManager<AppUser> _userManager,
                                 ITransferCenterService _transferCenterService) : Controller
    {
        private async Task LoadFormLokkupAsync()
        {
            var branches = await _branchService.GetAllAsync();
            ViewBag.branches = branches.Select(b => new SelectListItem
            {
                Text = b.Name,
                Value = b.Id.ToString()
            }).ToList();

            var users = await _userManager.Users.ToListAsync();
            ViewBag.users = users.Select(u => new SelectListItem
            {
                Text = $"{u.FirstName} {u.LastName} ({u.UserName})",
                Value = u.Id.ToString()
            }).ToList();

            ViewBag.cargoTypes = Enum.GetValues<CargoType>()
                .Select(t => new SelectListItem
                {
                    Text = t.ToString(),
                    Value = ((int)t).ToString()
                });

            ViewBag.packageSizes = Enum.GetValues<PackageSize>()
                .Select(s => new SelectListItem
                {
                    Text = PackageSizeRules.GetDisplayName(s),
                    Value = ((int)s).ToString()
                });
        }

        public async Task<IActionResult> Index(string? search, CargoStatus? status, int page = 1)
        {
            const int pageSize = 10;

            var cargos = await _cargoService.GetForManagerAsync(GetCurrentUserId());

            if (!string.IsNullOrWhiteSpace(search))
                cargos = cargos
                    .Where(c => c.TrackCode.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (status.HasValue)
                cargos = cargos.Where(c => c.CargoStatus == status.Value).ToList();

            var totalCount = cargos.Count;
            var items = cargos
                .OrderByDescending(c => c.ShipmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadFormLokkupAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCargoDto cargoDto)
        {
            if (!ModelState.IsValid)
            {
                await LoadFormLokkupAsync();
                return View(cargoDto);
            }

            await _cargoService.CreateAsync(cargoDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangeStatus(Guid id)
        {
            if (TempData["error"] != null)
                ViewBag.error = TempData["error"];


            try
            {
                var appUserId = GetCurrentUserId();
                var allowed = await _cargoService.GetAllowedNextStatusesAsync(id, appUserId);

                ViewBag.statuses = allowed.Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = ((int)s).ToString()
                }).ToList();

                var transferCenters = await _transferCenterService.GetAllAsync();
                ViewBag.transferCenters = transferCenters.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id.ToString()
                }).ToList();

                return View(new ChangeCargoStatusDto { CargoId = id });
            }
            catch (System.ComponentModel.DataAnnotations.ValidationException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(ChangeCargoStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return RedirectToAction(nameof(ChangeStatus), new { id = dto.CargoId });
            }

            try
            {
                await _cargoService.ChangeStatusAsync(dto, GetCurrentUserId());
            }
            catch (System.ComponentModel.DataAnnotations.ValidationException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction(nameof(ChangeStatus), new { id = dto.CargoId });
            }

            return RedirectToAction(nameof(Index));
        }

        private Guid GetCurrentUserId()
        {
            var userIdValue = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userIdValue))
                throw new UnauthorizedAccessException();

            return Guid.Parse(userIdValue);
        }
    
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var details = await _cargoService.GetDetailsForManagerAsync(id, GetCurrentUserId());
                ViewBag.CargoId = id;
                return View(details);
            }
            catch (ValidationException ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

            
        }

        public async Task<IActionResult> OutForDelivery()
        {
            var cargos = await _cargoService.GetForManagerAsync(GetCurrentUserId());
            var list = cargos
                .Where(c => c.CargoStatus == CargoStatus.OutForDelivery)
                .ToList();

            return View(list);
        }
        public async Task<IActionResult> Problematic()
        {
            var cargos = await _cargoService.GetForManagerAsync(GetCurrentUserId());
            var list = cargos
                .Where(c => c.CargoStatus == CargoStatus.DeliveryFailed || c.CargoStatus == CargoStatus.InReturnProcess)
                .ToList();

            return View(list);
        }
    }
}
