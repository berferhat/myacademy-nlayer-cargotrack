

using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.CargoDtos
{
    public class CreateCargoDto
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid OriginBranchId { get; set; }
        public Guid DestinationBranchId { get; set; }
        public double Weight { get; set; }
        public CargoType CargoType { get; set; }
        public PackageSize PackageSize { get; set; }
    }
}
