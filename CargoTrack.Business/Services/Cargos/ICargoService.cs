using CargoTrack.DTO.DTOs.CargoDtos;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Services.Cargos
{
    public interface ICargoService
    {
        Task<List<ResultCargoDto>> GetAllAsync();
        Task<ResultCargoDto> GetByIdAsync(Guid id);
        Task CreateAsync(CreateCargoDto createCargoDto);
        Task DeleteAsync(Guid id);
        Task<CargoTrackResultDto?> GetByTrackCodeAsync(string trackCode);
        Task ChangeStatusAsync(ChangeCargoStatusDto changeCargoStatusDto, Guid appUserId);
        Task<List<CargoStatus>> GetAllowedNextStatusesAsync(Guid cargoId, Guid appUserId);
        Task<List<ResultCargoDto>> GetForManagerAsync(Guid appUserId);
        Task<List<ResultCargoDto>> GetForUserAsync(Guid appUserId);
        Task<List<ResultCargoDto>> GetDelayedAsync();
        Task<CargoTrackResultDto> GetDetailsForManagerAsync(Guid cargoId, Guid appUserId);
        Task<CargoTrackResultDto> GetDetailsForUserAsync(Guid cargoId, Guid appUserId);
    }
}
