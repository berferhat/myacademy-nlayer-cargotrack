using CargoTrack.DataAccess.Context;
using CargoTrack.Entity.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.DataAccess.Repositories.GenericRepositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
