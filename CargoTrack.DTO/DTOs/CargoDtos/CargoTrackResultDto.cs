using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.CargoDtos
{
    public class CargoTrackResultDto
    {
        public string TrackCode { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public string? OriginCityName { get; set; }
        public string? DestinationCityName { get; set; }
        public string? OriginBranchName { get; set; }
        public string? DestinationBranchName { get; set; }
        public List<CargoMovementDto> Movements { get; set; }
        public string? DeliveryCode { get; set; }
    }

    public class CargoMovementDto
    {
        public CargoStatus NewStatus { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Description { get; set; }
        public string? BranchName { get; set; }
    }
}
