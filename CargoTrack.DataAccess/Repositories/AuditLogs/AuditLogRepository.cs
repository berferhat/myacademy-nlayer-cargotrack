using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.DataAccess.Repositories.AuditLogs
{
    public class AuditLogRepository(AppDbContext context) : GenericRepository<AuditLog>(context), IAuditLogRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<List<AuditLog>> GetFilteredAsync(
            string? entityName, 
            string? actionType, 
            DateTime? fromDate, 
            DateTime? toDate)
        {
            var query = _context.AuditLogs.Where(x => !x.IsDeleted);

            if(!string.IsNullOrWhiteSpace(entityName))
                query = query.Where(x => x.EntityName == entityName);

            if(!string.IsNullOrWhiteSpace(actionType))
                query = query.Where(x => x.ActionType == actionType);

            if(fromDate.HasValue)
                query = query.Where(x => x.ActionDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(x => x.ActionDate <= toDate.Value);

            return await query
                .OrderByDescending(x => x.ActionDate)
                .ToListAsync();
        }
    }
}
