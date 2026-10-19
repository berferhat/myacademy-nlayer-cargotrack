namespace CargoTrack.DTO.DTOs.DeliveryDtos
{
    public class CreateDeliveryDto
    {
        public Guid CargoId { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string? ReceivedByName { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
