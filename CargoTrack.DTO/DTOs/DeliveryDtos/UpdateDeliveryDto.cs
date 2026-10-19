namespace CargoTrack.DTO.DTOs.DeliveryDtos
{
    public class UpdateDeliveryDto
    {
        public Guid Id { get; set; }
        public Guid CargoId { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string? ReceivedByName { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
