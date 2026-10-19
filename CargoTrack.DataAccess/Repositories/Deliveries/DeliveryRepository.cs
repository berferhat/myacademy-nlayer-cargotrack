using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.Deliveries
{
    public class DeliveryRepository : GenericRepository<Delivery>, IDeliveryRepository
    {
        private readonly AppDbContext _context;

        public DeliveryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Delivery?> GetByCargoIdAsync(Guid cargoId)
        {
            return await _context.Deliveries.FirstOrDefaultAsync(x => x.CargoId == cargoId && !x.IsDeleted);
        }
    }
}
