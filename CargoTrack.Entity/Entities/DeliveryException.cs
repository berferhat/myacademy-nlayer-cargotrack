

using CargoTrack.Entity.Entities.Common;

namespace CargoTrack.Entity.Entities
{
    public class DeliveryException : BaseEntity
    {
        public Guid CargoId { get; set; }
        public string Reason { get; set; }
        public DateTime ExceptionDate { get; set; }
        public Guid? EmployeeId { get; set; }

        public virtual Cargo Cargo { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
