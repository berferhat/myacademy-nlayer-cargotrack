using CargoTrack.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class Delivery : BaseEntity
    {
        public Guid CargoId { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string? ReceivedByName { get; set; }
        public Guid? EmployeeId { get; set; }

        public virtual Cargo Cargo { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
