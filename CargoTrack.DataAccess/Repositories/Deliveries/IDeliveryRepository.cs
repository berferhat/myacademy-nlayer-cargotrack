using CargoTrack.DataAccess.Repositories.GenericRepositories;
using CargoTrack.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DataAccess.Repositories.Deliveries
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Task<Delivery?> GetByCargoIdAsync(Guid cargoId);
    }
}
