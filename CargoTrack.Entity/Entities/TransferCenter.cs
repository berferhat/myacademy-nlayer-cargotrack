using CargoTrack.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.Entity.Entities
{
    public class TransferCenter : BaseEntity
    {
        public string Name { get; set; }
        public Guid CityId { get; set; }

        public virtual City City { get; set; }
    }
}
