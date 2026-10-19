namespace CargoTrack.DTO.DTOs.DashboardDtos
{
    public class AdminDashboardDto
    {
        public int TotalCargos { get; set; }
        public int ActiveCargos { get; set; }
        public int DelayedCargos { get; set; }
        public int DeliveredCargos { get; set; }
        public double DeliveryRatePercent{ get; set; }

        public int BranchCount { get; set; }
        public int EmployeeCount { get; set; }

        public int Last7DaysCreatedCount { get; set; }

        // Chart.js: durum dağılımı
        public List<string> StatusLabels { get; set; } = new();
        public List<int> StatusCounts { get; set; } = new();

        // Chart.js: en yoğun 5 şube (çıkış)
        public List<string> TopBranchLabels { get; set; } = new();
        public List<int> TopBranchCounts{ get; set; } = new();
    }
}
