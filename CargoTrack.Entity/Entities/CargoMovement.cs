using CargoTrack.Entity.Entities.Common;
using CargoTrack.Entity.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class CargoMovement : BaseEntity
    {
        public Guid CargoId { get; set; }
        public CargoStatus? OldStatus { get; set; }
        public CargoStatus NewStatus { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Description { get; set; }

        public Guid? BranchId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? TransferCenterId { get; set; }

        // Navigation
        public virtual Cargo Cargo { get; set; }
        public virtual Branch? Branch { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual TransferCenter? TransferCenter { get; set; }
    }
}
