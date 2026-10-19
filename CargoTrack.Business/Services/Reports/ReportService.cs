using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DTO.DTOs.ReportDtos;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Services.Reports
{
    public class ReportService(
        ICargoRepository _cargoRepository,
        IBranchRepository _branchRepository) : IReportService
    {
        public async Task<AdminReportDto> GetAdminReportAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            var branches = await _branchRepository.GetAllAsync();
            var delayed = await _cargoRepository.GetDelayedAsync();

            string BranchName(Guid id) => branches.FirstOrDefault(b => b.Id == id)?.Name ?? id.ToString();

            var total = cargos.Count;
            var delivered = cargos.Count(c => c.CargoStatus == CargoStatus.Delivered);
            var returned = cargos.Count(c => c.CargoStatus == CargoStatus.ReturnedToSender);

            var topSend = cargos
                .GroupBy(c => c.OriginBranchId)
                .Select(g => new BranchStatDto { BranchName = BranchName(g.Key), Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            var topRecv = cargos
                .GroupBy(c => c.DestinationBranchId)
                .Select(g => new BranchStatDto { BranchName = BranchName(g.Key), Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            var delayedBranches = delayed
               .GroupBy(c => c.DestinationBranchId)
               .Select(g => new BranchStatDto { BranchName = BranchName(g.Key), Count = g.Count() })
               .OrderByDescending(x => x.Count)
               .Take(5)
               .ToList();

            var since = DateTime.UtcNow.Date.AddDays(-6);
            var volume = Enumerable.Range(0, 7)
                .Select(i => since.AddDays(i))
                .Select(day => new DailyVolumeDto
                {
                    DayLabel = day.ToString("dd.MM"),
                    Count = cargos.Count(c => c.ShipmentDate.Date == day)
                }).ToList();

            return new AdminReportDto
            {
                DeliverySuccessPercent = total == 0 ? 0 : Math.Round(delivered * 100.0 / total, 1),
                ReturnPercent = total == 0 ? 0 : Math.Round(returned * 100.0 / total, 1),
                DelayedCargoCount = delayed.Count,
                TopSendingBranches = topSend,
                TopReceivingBranches = topRecv,
                DelayedBranches = delayedBranches,
                Last7DaysVolume = volume
            };
        }
    }
}
