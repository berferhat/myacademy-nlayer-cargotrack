using CargoTrack.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Title { get; set; }

        public Guid BranchId { get; set; }
        public Guid? AppUserId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual AppUser? AppUser { get; set; }
    }
}
