using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Employees;
using CargoTrack.DTO.DTOs.EmployeeDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CargoTrack.WebUI.Areas.Admin.Controllers
{
    [Area(Area.Admin)]
    [Authorize(Roles = Roles.Admin)]
    public class EmployeeController(
        IEmployeeService _employeeService,
        IBranchService _branchService,
        UserManager<AppUser> _userManager) : Controller
    {
        private async Task LoadLookupAsync()
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
        }

        public async Task<IActionResult> Index()
        {
            var list = await _employeeService.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadLookupAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupAsync();
                return View(dto);
            }

            await _employeeService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            await LoadLookupAsync();
            var item = await _employeeService.GetByIdAsync(id);

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeDto dto)
        {
            if(!ModelState.IsValid)
            {
                await LoadLookupAsync();
                return View(dto);
            }

            await _employeeService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _employeeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
