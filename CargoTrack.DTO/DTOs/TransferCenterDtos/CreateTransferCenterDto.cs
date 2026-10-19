using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrack.DTO.DTOs.TransferCenterDtos
{
    public class CreateTransferCenterDto
    {
        public string Name { get; set; }
        public Guid CityId { get; set; }
    }
}
