using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.CargoDtos
{
    public class ResultCargoDto
    {
        // Kimlik
        public Guid Id { get; set; }
        public string TrackCode { get; set; }
        // Tarihler
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        // Ölçü/Fiyat
        public double Weight { get; set; }
        public double? Desi { get; set; }
        public decimal? Price { get; set; }
        // Enum'lar
        public CargoType CargoType { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public PackageSize PackageSize { get; set; }
        // Şubeler
        public Guid OriginBranchId { get; set; }
        public Guid DestinationBranchId { get; set; }
        public string? OriginBranchName { get; set; }
        public string? DestinationBranchName { get; set; }
        // Teslim
        public string? DeliveryCode { get; set; }
        // Taraflar (Incoming / Outgoing filtresi)
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}
