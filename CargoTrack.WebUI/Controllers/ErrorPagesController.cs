using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.Controllers
{
    public class ErrorPagesController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error(string? message)
        {
            ViewBag.Message = message ?? "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
            return View();
        }
    }
}
