using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;


namespace CargoTrack.DataAccess.Repositories.Cargos
{
    public interface ICargoRepository : IRepository<Cargo>
    {
        Task<Cargo?> GetByTrackCodeAsync(string trackCode);
        Task<Cargo?> GetByIdWithMovementsAsync(Guid id);
        Task SaveChangesAsync();
        Task<List<Cargo>> GetByBranchIdAsync(Guid branchId);
        Task<List<Cargo>> GetByUserIdAsync(Guid appUserId);
        Task<List<Cargo>> GetDelayedAsync();
    }
}
