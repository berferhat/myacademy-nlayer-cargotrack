
using CargoTrack.Business.Services.CargoPricings;
using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.CargoDtos;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.Cargos
{
    public class CargoService(ICargoRepository _cargoRepository,
                              ICargoPricingService _cargoPricingService,
                              IEmployeeRepository _employeeRepository,
                              IAuditLogRepository _auditLogRepository) : ICargoService
    {
        public async Task CreateAsync(CreateCargoDto createCargoDto)
        {
            var cargo = createCargoDto.Adapt<Cargo>();

            cargo.Desi = PackageSizeRules.GetBillableDesi(createCargoDto.PackageSize);
            cargo.Price = await _cargoPricingService.CalculateAsync(new CargoPriceCalculationDto
            {
                CargoType = createCargoDto.CargoType,
                Weight = createCargoDto.Weight,
                Desi = cargo.Desi.Value
            });
            cargo.TrackCode = GenerateTrackCode();
            cargo.CargoStatus = CargoStatus.Created;
            cargo.ShipmentDate = DateTime.UtcNow;
            cargo.EstimatedArrivalDate = cargo.CargoType switch
            {
                CargoType.Urgent => DateTime.UtcNow.AddDays(1),
                CargoType.Standard => DateTime.UtcNow.AddDays(3),
                CargoType.Fragile => DateTime.UtcNow.AddDays(3),
                CargoType.Heavy => DateTime.UtcNow.AddDays(5),
                _ => DateTime.UtcNow.AddDays(3)
            };
            cargo.FailedDeliveryAttemptCount = 0;

            cargo.Movements = new List<CargoMovement>
            {
                new CargoMovement
                {
                    OldStatus = null,
                    NewStatus = CargoStatus.Created,
                    MovementDate = DateTime.UtcNow,
                    Description = "Kargo oluşturuldu.",
                    BranchId = createCargoDto.OriginBranchId
                }
            };

            await _cargoRepository.CreateAsync(cargo);
        }

        public async Task DeleteAsync(Guid id)
        {
            var cargo = await _cargoRepository.GetByIdAsync(id);

            if (cargo is null)
                throw new ValidationException("Kargo bulunamadı");

            await _cargoRepository.DeleteAsync(cargo);
        }

        public async Task<List<ResultCargoDto>> GetAllAsync()
        {
            var cargos = await _cargoRepository.GetAllAsync();
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<ResultCargoDto> GetByIdAsync(Guid id)
        {
            var cargo = await _cargoRepository.GetByIdAsync(id);

            if (cargo is null)
                throw new ValidationException("Kargo bulunamadı");

            return cargo.Adapt<ResultCargoDto>();
        }

        public async Task<CargoTrackResultDto?> GetByTrackCodeAsync(string trackCode)
        {
            var cargo = await _cargoRepository.GetByTrackCodeAsync(trackCode);

            if (cargo is null)
                return null;

            return new CargoTrackResultDto
            {
                TrackCode = cargo.TrackCode,
                CargoStatus = cargo.CargoStatus,
                ShipmentDate = cargo.ShipmentDate,
                EstimatedArrivalDate = cargo.EstimatedArrivalDate,
                OriginBranchName = cargo.OriginBranch?.Name,
                DestinationBranchName = cargo.DestinationBranch?.Name,
                OriginCityName = cargo.OriginBranch?.City?.Name,
                DestinationCityName = cargo.DestinationBranch?.City?.Name,
                Movements = cargo.Movements?
                    .OrderBy(m => m.MovementDate)
                    .Select(m => new CargoMovementDto
                    {
                        NewStatus = m.NewStatus,
                        MovementDate = m.MovementDate,
                        Description = m.Description,
                        BranchName = m.Branch?.Name
                    }).ToList() ?? new List<CargoMovementDto>()
            };
        }

        private static string GenerateTrackCode()
        {
            // Örn: TR + yıl + 6 haneli rastgele
            return $"MYC-{DateTime.UtcNow:yyyy}-{Random.Shared.Next(100000, 999999)}";
        }

        public async Task ChangeStatusAsync(ChangeCargoStatusDto changeCargoStatusDto, Guid appUserId)
        {
            var cargo = await _cargoRepository.GetByIdWithMovementsAsync(changeCargoStatusDto.CargoId);

            if (cargo is null)
                throw new ValidationException("Kargo bulunamadı.");

            await EnsureManagerCanAccessCargoAsync(cargo, appUserId);
            var oldStatus = cargo.CargoStatus;
            EnsureAllowedTransition(oldStatus, changeCargoStatusDto.NewStatus, cargo.FailedDeliveryAttemptCount);

            if (changeCargoStatusDto.NewStatus == CargoStatus.AtTransferCenter && !changeCargoStatusDto.TransferCenterId.HasValue)
                throw new ValidationException("Transfer merkezine geçişte merkez seçilmelidir.");

            cargo.CargoStatus = changeCargoStatusDto.NewStatus;

            if (changeCargoStatusDto.NewStatus == CargoStatus.OutForDelivery)
                cargo.DeliveryCode = Random.Shared.Next(100000, 999999).ToString();

            if (changeCargoStatusDto.NewStatus == CargoStatus.DeliveryFailed)
            {
                if (string.IsNullOrWhiteSpace(changeCargoStatusDto.Description))
                    throw new ValidationException("Teslim edilememe nedeni zorunludur.");

                cargo.FailedDeliveryAttemptCount++;
                cargo.DeliveryExceptions ??= new List<DeliveryException>();
                cargo.DeliveryExceptions.Add(new DeliveryException
                {
                    CargoId = cargo.Id,
                    Reason = changeCargoStatusDto.Description.Trim(),
                    ExceptionDate = DateTime.UtcNow
                });
            }

            Guid? branchId = changeCargoStatusDto.BranchId;

            if (changeCargoStatusDto.NewStatus == CargoStatus.AtOriginBranch)
                branchId = cargo.OriginBranchId;
            else if (changeCargoStatusDto.NewStatus is CargoStatus.AtDestinationBranch
                or CargoStatus.OutForDelivery
                or CargoStatus.Delivered
                or CargoStatus.DeliveryFailed)
                branchId = cargo.DestinationBranchId;


            // Movement listesi null gelebilir.
            cargo.Movements ??= new List<CargoMovement>();
            cargo.Movements.Add(new CargoMovement
            {
                OldStatus = oldStatus,
                NewStatus = changeCargoStatusDto.NewStatus,
                MovementDate = DateTime.UtcNow,
                Description = changeCargoStatusDto.Description ?? $"Durum: {oldStatus} -> {changeCargoStatusDto.NewStatus}",
                BranchId = branchId,
                TransferCenterId = changeCargoStatusDto.TransferCenterId
            });

            if (changeCargoStatusDto.NewStatus == CargoStatus.Delivered)
            {
                if (string.IsNullOrWhiteSpace(cargo.DeliveryCode))
                    throw new ValidationException("Teslimat kodu üretilmemiş. Önce dağıtıma çıkılmalıdır.");

                if (string.IsNullOrWhiteSpace(changeCargoStatusDto.DeliveryCode))
                    throw new ValidationException("Teslimat kodu girilmelidir.");

                if (changeCargoStatusDto.DeliveryCode.Trim() != cargo.DeliveryCode)
                    throw new ValidationException("Teslimat kodu hatalı.");

                if (string.IsNullOrWhiteSpace(changeCargoStatusDto.ReceivedByName))
                    throw new ValidationException("Teslim alan kişi adı zorunludur.");

                cargo.Delivery = new Delivery
                {
                    CargoId = cargo.Id,
                    DeliveredAt = DateTime.UtcNow,
                    ReceivedByName = changeCargoStatusDto.ReceivedByName.Trim()
                };
            }

            await _cargoRepository.SaveChangesAsync();

            var auditLog = new AuditLog
            {
                UserId = appUserId,
                ActionType = "StatusChange",
                EntityName = "Cargo",
                EntityId = cargo.Id.ToString(),
                OldValues = oldStatus.ToString(),
                NewValues = changeCargoStatusDto.NewStatus.ToString(),
                Description = $"{cargo.TrackCode}: {oldStatus} → {changeCargoStatusDto.NewStatus}",
                ActionDate = DateTime.UtcNow
            };
            await _auditLogRepository.CreateAsync(auditLog);
        }

        private async Task EnsureManagerCanAccessCargoAsync(Cargo cargo, Guid appUserId)
        {
            var employee = await _employeeRepository.GetByAppUserIdAsync(appUserId);

            if (employee is null)
                throw new ValidationException("Bu kullanıcıya bağlı çalışan/şube kaydı bulunamadı.");

            var branchId = employee.BranchId;

            if (cargo.OriginBranchId != branchId && cargo.DestinationBranchId != branchId)
                throw new ValidationException("Bu kargo bağlı olduğunuz şubeye ait değil.");
        }

        private static void EnsureAllowedTransition(CargoStatus current, CargoStatus next, int failedDeliveryAttemptCount)
        {
            if (current == next)
                throw new ValidationException("Kargo zaten bu durumda.");

            // Bitti: geri dönüş yok
            if (current is CargoStatus.Delivered or CargoStatus.ReturnedToSender)
                throw new ValidationException("Tamamlanmış kargonun durumu değiştirilemez.");

            var allowed = GetAllowedNextStatuses(current, failedDeliveryAttemptCount);

            if (!allowed.Contains(next))
                throw new ValidationException($"Geçersiz geçiş: {current} → {next}");
        }

        public async Task<List<CargoStatus>> GetAllowedNextStatusesAsync(Guid cargoId, Guid appUserId)
        {
            var cargo = await _cargoRepository.GetByIdAsync(cargoId);

            if (cargo is null)
                throw new ValidationException("Kargo bulunamadı.");

            await EnsureManagerCanAccessCargoAsync(cargo, appUserId);

            return GetAllowedNextStatuses(cargo.CargoStatus, cargo.FailedDeliveryAttemptCount).ToList();
        }

        private static CargoStatus[] GetAllowedNextStatuses(CargoStatus current, int failedDeliveryAttemptCount)
        {
            if (current is CargoStatus.Delivered or CargoStatus.ReturnedToSender)
                return Array.Empty<CargoStatus>();

            return current switch
            {
                CargoStatus.Created => new[] { CargoStatus.AtOriginBranch },
                CargoStatus.AtOriginBranch => new[] { CargoStatus.AtTransferCenter },
                CargoStatus.AtTransferCenter => new[] { CargoStatus.AtDestinationBranch },
                CargoStatus.AtDestinationBranch => new[] { CargoStatus.OutForDelivery },
                CargoStatus.OutForDelivery => new[] { CargoStatus.Delivered, CargoStatus.DeliveryFailed },
                CargoStatus.DeliveryFailed => failedDeliveryAttemptCount >= 3
                    ? new[] { CargoStatus.InReturnProcess }
                    : new[] { CargoStatus.OutForDelivery, CargoStatus.InReturnProcess },
                CargoStatus.InReturnProcess => new[] { CargoStatus.ReturnedToSender },
                _ => Array.Empty<CargoStatus>()
            };
        }

        public async Task<List<ResultCargoDto>> GetForManagerAsync(Guid appUserId)
        {
            var employee = await _employeeRepository.GetByAppUserIdAsync(appUserId);

            if (employee is null)
                throw new ValidationException("Bu kullanıcıya bağlı çalışan/şube kaydı bulunamadı.");

            var cargos = await _cargoRepository.GetByBranchIdAsync(employee.BranchId);

            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetForUserAsync(Guid appUserId)
        {
            var cargos = await _cargoRepository.GetByUserIdAsync(appUserId);
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<List<ResultCargoDto>> GetDelayedAsync()
        {
            var cargos = await _cargoRepository.GetDelayedAsync();
            return cargos.Adapt<List<ResultCargoDto>>();
        }

        public async Task<CargoTrackResultDto> GetDetailsForManagerAsync(Guid cargoId, Guid appUserId)
        {
            var cargo = await _cargoRepository.GetByIdWithMovementsAsync(cargoId);

            if (cargo is null)
                throw new ValidationException("Kargo bulunamadı.");

            await EnsureManagerCanAccessCargoAsync(cargo, appUserId);

            return new CargoTrackResultDto
            {
                TrackCode = cargo.TrackCode,
                DeliveryCode = cargo.DeliveryCode,
                CargoStatus = cargo.CargoStatus,
                ShipmentDate = cargo.ShipmentDate,
                EstimatedArrivalDate = cargo.EstimatedArrivalDate,
                OriginBranchName = cargo.OriginBranch?.Name,
                DestinationBranchName = cargo.DestinationBranch?.Name,
                OriginCityName = cargo.OriginBranch?.City?.Name,
                DestinationCityName = cargo.DestinationBranch?.City?.Name,
                Movements = cargo.Movements?
                    .OrderBy(m => m.MovementDate)
                    .Select(m => new CargoMovementDto
                    {
                        NewStatus = m.NewStatus,
                        MovementDate = m.MovementDate,
                        Description = m.Description,
                        BranchName = m.Branch?.Name
                    }).ToList() ?? new List<CargoMovementDto>()
            };
        }

        public async Task<CargoTrackResultDto> GetDetailsForUserAsync(Guid cargoId, Guid appUserId)
        {
            var cargo = await _cargoRepository.GetByIdWithMovementsAsync(cargoId);

            if (cargo is null)
                throw new ValidationException("Kargo bulunamadı.");

            if (cargo.SenderId != appUserId && cargo.ReceiverId != appUserId)
                throw new ValidationException("Bu kargoyu görüntüleme yetkiniz yok.");

            return new CargoTrackResultDto
            {
                TrackCode = cargo.TrackCode,
                CargoStatus = cargo.CargoStatus,
                ShipmentDate = cargo.ShipmentDate,
                EstimatedArrivalDate = cargo.EstimatedArrivalDate,
                OriginBranchName = cargo.OriginBranch?.Name,
                DestinationBranchName = cargo.DestinationBranch?.Name,
                OriginCityName = cargo.OriginBranch?.City?.Name,
                DestinationCityName = cargo.DestinationBranch?.City?.Name,
                Movements = cargo.Movements?
                .OrderBy(m => m.MovementDate)
                .Select(m => new CargoMovementDto
                {
                    NewStatus = m.NewStatus,
                    MovementDate = m.MovementDate,
                    Description = m.Description,
                    BranchName = m.Branch?.Name
                }).ToList() ?? new List<CargoMovementDto>()
            };
        }
    }
}
