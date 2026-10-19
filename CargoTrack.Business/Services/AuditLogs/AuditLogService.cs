using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.DTO.DTOs.AuditLogDtos;
using CargoTrack.Entity.Entities;
using Mapster;

namespace CargoTrack.Business.Services.AuditLogs
{
    public class AuditLogService(IAuditLogRepository _auditLogRepository) : IAuditLogService
    {
        public async Task CreateAsync(CreateAuditLogDto createAuditLogDto)
        {
            var auditLog = createAuditLogDto.Adapt<AuditLog>();
            await _auditLogRepository.CreateAsync(auditLog);
        }

        public async Task<List<ResultAuditLogDto>> GetAllAsync()
        {
            var logs = await _auditLogRepository.GetAllAsync();
            return logs.Adapt<List<ResultAuditLogDto>>();
        }

        public async Task<List<ResultAuditLogDto>> GetFilteredAsync(
            string? entityName, 
            string? actionType, 
            DateTime? fromDate, 
            DateTime? toDate)
        {
            var logs = await _auditLogRepository.GetFilteredAsync(
                entityName, actionType, fromDate, toDate);
            
            return logs.Adapt<List<ResultAuditLogDto>>();
        }
    }
}
