using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.CargoPricings
{
    public class CargoPricingService(ICargoPriceRepository _cargoPriceRepository) : ICargoPricingService
    {
        public async Task<decimal> CalculateAsync(CargoPriceCalculationDto dto)
        {
            var cargoPrice = await _cargoPriceRepository.GetActiveByTypeAsync(dto.CargoType);

            if (cargoPrice is null)
                throw new ValidationException("Bu kargo tipi için aktif fiyat tanımı yok.");

            var chargeable = (decimal)Math.Max(dto.Weight, dto.Desi);
            var unitPrice = (decimal)dto.Weight >= (decimal)dto.Desi
                ? cargoPrice.PricePerKg
                : cargoPrice.PricePerDesi;

            return cargoPrice.BasePrice + chargeable * unitPrice;
        }
    }
}
