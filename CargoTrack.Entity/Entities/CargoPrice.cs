
using CargoTrack.Entity.Entities.Common;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Entity.Entities
{
    public class CargoPrice : BaseEntity
    {
        public string Name { get; set; }
        public CargoType? CargoType { get; set; }
        public decimal BasePrice { get; set; }
        public decimal PricePerKg { get; set; }
        public decimal PricePerDesi { get; set; }
        public bool IsActive { get; set; }
    }
}
