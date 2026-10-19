using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class ManagerDashboardDto
    {
        public int TodayIncoming { get; set; }
        public int TodayOutgoing { get; set; }
        public int OutForDeliveryCount { get; set; }
        public int DeliveredCount { get; set; }
        public int DelayedCount { get; set; }
        public int ProblematicCount { get; set; }

        public List<ManagerDashboardCargoItemDto> RecentCargos { get; set; } = new();
    }

    public class ManagerDashboardCargoItemDto
    {
        public Guid Id { get; set; }
        public string TrackCode { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
