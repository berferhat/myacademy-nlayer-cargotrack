using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.CargoDtos
{
    public class ChangeCargoStatusDto
    {
        public Guid CargoId { get; set; }
        public CargoStatus NewStatus { get; set; }
        public Guid? BranchId { get; set; }
        public Guid? TransferCenterId { get; set; }
        public string? Description { get; set; }
        public string? ReceivedByName { get; set; }
        public string? DeliveryCode { get; set; }
    }
}
