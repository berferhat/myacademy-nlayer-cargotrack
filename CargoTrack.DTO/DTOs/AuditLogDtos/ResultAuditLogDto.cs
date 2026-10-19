namespace CargoTrack.DTO.DTOs.AuditLogDtos
{
    public class ResultAuditLogDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string ActionType { get; set; }
        public string EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? Description { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
