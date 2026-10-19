using CargoTrack.Entity.Entities.Common;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Entity.Entities
{
    public class Cargo : BaseEntity
    {
        public string TrackCode { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public double Weight { get; set; }
        public CargoType CargoType { get; set; }
        public PackageSize PackageSize { get; set; }
        public CargoStatus CargoStatus { get; set; }

        public double? Desi { get; set; }
        public decimal? Price { get; set; }
        public string? DeliveryCode { get; set; }
        public int FailedDeliveryAttemptCount { get; set; }

        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid OriginBranchId { get; set; }
        public Guid DestinationBranchId { get; set; }

        // Navigation Properties
        public virtual AppUser Sender { get; set; }
        public virtual AppUser Receiver { get; set; }
        public virtual Branch OriginBranch { get; set; }
        public virtual Branch DestinationBranch { get; set; }
        public virtual IList<CargoMovement> Movements { get; set; }
        public virtual Delivery? Delivery { get; set; }
        public virtual IList<DeliveryException> DeliveryExceptions { get; set; }
        
       
    }
}
