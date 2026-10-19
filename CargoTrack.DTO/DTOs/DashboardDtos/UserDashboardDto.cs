
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class UserDashboardDto
    {
        public int ActiveCount { get; set; }
        public int HistoryCount { get; set; }

        public string? LatestTrackCode { get; set; }
        public CargoStatus? LatestStatus { get; set; }
        public DateTime? LatestEta { get; set; }

        public List<UserDashboardCargoItemDto> RecentCargos { get; set; } = new();
    }

    public class UserDashboardCargoItemDto
    {
        public Guid Id { get; set; }
        public string TrackCode { get; set; }
        public CargoStatus CargoStatus { get; set; }
        public DateTime EstimatedArrivalDate { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
