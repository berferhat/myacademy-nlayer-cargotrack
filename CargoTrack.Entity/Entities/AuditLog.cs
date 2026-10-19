using CargoTrack.Entity.Entities.Common;

namespace CargoTrack.Entity.Entities
{
    public class AuditLog : BaseEntity
    {
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