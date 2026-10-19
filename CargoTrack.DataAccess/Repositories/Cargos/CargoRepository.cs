using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.DataAccess.Repositories.Cargos
{
    public class CargoRepository(AppDbContext context) : GenericRepository<Cargo>(context), ICargoRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Cargo?> GetByTrackCodeAsync(string trackCode)
        {
            return await _context.Cargos
                .Include(x => x.OriginBranch).ThenInclude(b => b.City)
                .Include(x => x.DestinationBranch).ThenInclude(b => b.City)
                .Include(x => x.Movements).ThenInclude(m => m.Branch)
                .FirstOrDefaultAsync(x => x.TrackCode == trackCode && !x.IsDeleted);
        }

        public async Task<Cargo?> GetByIdWithMovementsAsync(Guid id)
        {
            return await _context.Cargos
                .Include(x => x.Movements)
                .Include(x => x.DeliveryExceptions)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cargo>> GetByBranchIdAsync(Guid branchId)
        {
            return await _context.Cargos
                .Where(c => !c.IsDeleted && (c.OriginBranchId == branchId || c.DestinationBranchId == branchId))
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetByUserIdAsync(Guid appUserId)
        {
            return await _context.Cargos
                .Where(c => !c.IsDeleted && (c.SenderId == appUserId || c.ReceiverId == appUserId))
                .ToListAsync();
        }

        public async Task<List<Cargo>> GetDelayedAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.Cargos
                .Where(c => !c.IsDeleted && c.EstimatedArrivalDate < now && c.CargoStatus != CargoStatus.Delivered)
                .ToListAsync();
        }
    }
}
