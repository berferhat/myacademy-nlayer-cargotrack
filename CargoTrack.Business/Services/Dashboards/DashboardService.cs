using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.DashboardDtos;
using CargoTrack.Entity.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.Dashboards
{
    public class DashboardService(
        ICargoRepository _cargoRepository,
        IBranchRepository _branchRepository,
        IEmployeeRepository _employeeRepository) : IDashboardService
    {
        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            var delayed = await _cargoRepository.GetDelayedAsync();
            var branches = await _branchRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();

            var delivered = cargos.Count(c => c.CargoStatus == CargoStatus.Delivered);
            var active = cargos.Count(c =>
                c.CargoStatus != CargoStatus.Delivered &&
                c.CargoStatus != CargoStatus.ReturnedToSender);

            var since = DateTime.UtcNow.AddDays(-7);
            var last7 = cargos.Count(c => c.ShipmentDate >= since);

            var statusGroups = cargos
                .GroupBy(c => c.CargoStatus)
                .OrderBy(g => g.Key)
                .ToList();

            var topBranches = cargos
                .GroupBy(c => c.OriginBranchId)
                .Select(g => new
                {
                    BranchId = g.Key,
                    Count = g.Count(),
                    Name = branches.FirstOrDefault(b => b.Id == g.Key)?.Name ?? g.Key.ToString()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            return new AdminDashboardDto
            {
                TotalCargos = cargos.Count,
                ActiveCargos = active,
                DelayedCargos = delayed.Count,
                DeliveredCargos = delivered,
                DeliveryRatePercent = cargos.Count == 0 ? 0 : Math.Round(delivered * 100.0 / cargos.Count, 1),

                BranchCount = branches.Count,
                EmployeeCount = employees.Count,
                Last7DaysCreatedCount = last7,

                StatusLabels = statusGroups.Select(g => g.Key.ToString()).ToList(),
                StatusCounts = statusGroups.Select(g => g.Count()).ToList(),

                TopBranchLabels = topBranches.Select(x => x.Name).ToList(),
                TopBranchCounts = topBranches.Select(x => x.Count).ToList(),
            };
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardAsync(Guid appUserId)
        {
            var employee = await _employeeRepository.GetByAppUserIdAsync(appUserId);

            if (employee is null)
                throw new ValidationException("Bu kullanıcıya bağlı çalışan/şube kaydı bulunamadı.");

            var branchId = employee.BranchId;
            var cargos = await _cargoRepository.GetByBranchIdAsync(branchId);
            var today = DateTime.UtcNow.Date;

            var delayedAll = await _cargoRepository.GetDelayedAsync();
            var delayed = delayedAll.Count(c => c.OriginBranchId == branchId || c.DestinationBranchId == branchId);

            return new ManagerDashboardDto
            {
                TodayIncoming = cargos.Count(c => c.DestinationBranchId == branchId && c.ShipmentDate.Date == today),
                TodayOutgoing = cargos.Count(c => c.OriginBranchId == branchId && c.ShipmentDate.Date == today),
                OutForDeliveryCount = cargos.Count(c => c.CargoStatus == CargoStatus.OutForDelivery),
                DeliveredCount = cargos.Count(c => c.CargoStatus == CargoStatus.Delivered),
                DelayedCount = delayed,
                ProblematicCount = cargos.Count(c =>
                    c.CargoStatus == CargoStatus.DeliveryFailed ||
                    c.CargoStatus == CargoStatus.InReturnProcess),
                RecentCargos = cargos
                    .OrderByDescending(c => c.ShipmentDate)
                    .Take(5)
                    .Select(c => new ManagerDashboardCargoItemDto
                    {

                        Id = c.Id,
                        TrackCode = c.TrackCode,
                        CargoStatus = c.CargoStatus,
                        ShipmentDate = c.ShipmentDate
                    }).ToList()
            };
        }

        public async Task<UserDashboardDto> GetUserDashboardAsync(Guid appUserId)
        {
            var cargos = await _cargoRepository.GetByUserIdAsync(appUserId);

            var active = cargos
                .Where(c => c.CargoStatus != CargoStatus.Delivered &&
                             c.CargoStatus != CargoStatus.ReturnedToSender)
                .ToList();

            var history = cargos
                .Where(c => c.CargoStatus == CargoStatus.Delivered || 
                            c.CargoStatus == CargoStatus.ReturnedToSender)
                .ToList();

            var latest = cargos
                .OrderByDescending(c => c.ShipmentDate)
                .FirstOrDefault();

            return new UserDashboardDto
            {
                ActiveCount = active.Count,
                HistoryCount = history.Count,
                LatestTrackCode = latest?.TrackCode,
                LatestStatus = latest?.CargoStatus,
                LatestEta = latest?.EstimatedArrivalDate,
                RecentCargos = cargos
            .OrderByDescending(c => c.ShipmentDate)
            .Take(5)
            .Select(c => new UserDashboardCargoItemDto
                {
                    Id = c.Id,
                    TrackCode = c.TrackCode,
                    CargoStatus = c.CargoStatus,
                    EstimatedArrivalDate = c.EstimatedArrivalDate,
                    ShipmentDate = c.ShipmentDate
                }).ToList()
            };
        }
    }
}
