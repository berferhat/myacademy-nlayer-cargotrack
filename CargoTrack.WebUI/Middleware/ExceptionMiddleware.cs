using System.ComponentModel.DataAnnotations;

namespace CargoTrack.WebUI.Middleware
{
    public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
			try
			{
				await _next(context);
			}
			catch (ValidationException ex)
			{
				_logger.LogWarning(ex, "Doğrulama hatası");
				context.Response.Redirect($"/ErrorPages/Error?message={Uri.EscapeDataString(ex.Message)}");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Beklenmeyen hata");
				context.Response.Redirect("/ErrorPages/Error");
			}
        }
    }
}
