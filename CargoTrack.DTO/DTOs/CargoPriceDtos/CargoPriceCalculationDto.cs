using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.CargoPriceDtos
{
    public class CargoPriceCalculationDto
    {
        public CargoType CargoType { get; set; }
        public double Weight { get; set; }
        public double Desi { get; set; }
    }
}
