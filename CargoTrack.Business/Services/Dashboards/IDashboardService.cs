using CargoTrack.DTO.DTOs.DashboardDtos;

namespace CargoTrack.Business.Services.Dashboards
{
    public interface IDashboardService
    {
        Task<AdminDashboardDto> GetAdminDashboardAsync();
        Task<ManagerDashboardDto> GetManagerDashboardAsync(Guid appUserId);
        Task<UserDashboardDto> GetUserDashboardAsync(Guid appUserId);
    }
}
