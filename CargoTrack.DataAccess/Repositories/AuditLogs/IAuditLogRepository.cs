using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;

namespace CargoTrack.DataAccess.Repositories.AuditLogs
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<List<AuditLog>> GetFilteredAsync(
            string? entityName, 
            string? actionType, 
            DateTime? fromDate, 
            DateTime? toDate);
    }
}
