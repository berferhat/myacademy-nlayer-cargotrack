using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.CargoPrices
{
    public class CargoPriceRepository(AppDbContext context) : GenericRepository<CargoPrice>(context), ICargoPriceRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<CargoPrice?> GetActiveByTypeAsync(CargoType cargoType)
        {
            return await _context.CargoPrices.FirstOrDefaultAsync(x => x.IsActive && !x.IsDeleted && x.CargoType == cargoType);         }
    }
}
