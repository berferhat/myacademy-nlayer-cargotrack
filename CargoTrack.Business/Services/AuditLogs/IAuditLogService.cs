using CargoTrack.DTO.DTOs.AuditLogDtos;

namespace CargoTrack.Business.Services.AuditLogs
{
    public interface IAuditLogService
    {
        Task CreateAsync(CreateAuditLogDto createAuditLogDto);
        Task<List<ResultAuditLogDto>> GetAllAsync();
        Task<List<ResultAuditLogDto>> GetFilteredAsync(
            string? entityName,
            string? actionType,
            DateTime? fromDate,
            DateTime? toDate);

    }
}
