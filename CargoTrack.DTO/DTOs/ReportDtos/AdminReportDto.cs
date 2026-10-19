namespace CargoTrack.DTO.DTOs.ReportDtos
{
    public class AdminReportDto
    {
        public double DeliverySuccessPercent { get; set; }
        public double ReturnPercent { get; set; }
        public int DelayedCargoCount { get; set; }

        public List<BranchStatDto> TopSendingBranches { get; set; } = new();
        public List<BranchStatDto> TopReceivingBranches { get; set; } = new();
        public List<BranchStatDto> DelayedBranches { get; set; } = new();
        public List<DailyVolumeDto> Last7DaysVolume { get; set; } = new();
    }

    public class BranchStatDto
    {
        public string BranchName { get; set; }
        public int Count { get; set; }
    }

    public class DailyVolumeDto
    {
        public string DayLabel { get; set; }
        public int Count { get; set; }
    }
}
