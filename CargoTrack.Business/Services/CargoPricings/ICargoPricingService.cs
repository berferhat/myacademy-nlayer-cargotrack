using CargoTrack.DTO.DTOs.CargoPriceDtos;

namespace CargoTrack.Business.Services.CargoPricings
{
    public interface ICargoPricingService
    {
        Task<decimal> CalculateAsync(CargoPriceCalculationDto dto);
    }
}
