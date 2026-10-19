using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DTO.DTOs.CargoPriceDtos;
using CargoTrack.Entity.Entities;
using Mapster;
using System.ComponentModel.DataAnnotations;

namespace CargoTrack.Business.Services.CargoPrices
{
    public class CargoPriceService(ICargoPriceRepository _cargoPriceRepository) : ICargoPriceService
    {
        public async Task CreateAsync(CreateCargoPriceDto createCargoPriceDto)
        {
            var cargoPrice = createCargoPriceDto.Adapt<CargoPrice>();
            await _cargoPriceRepository.CreateAsync(cargoPrice);
        }

        public async Task DeleteAsync(Guid id)
        {
            var cargoPrice = await _cargoPriceRepository.GetByIdAsync(id);

            if (cargoPrice is null)
                throw new ValidationException("Fiyat tanımı bulunamadı.");

            await _cargoPriceRepository.DeleteAsync(cargoPrice);
        }

        public async Task<List<ResultCargoPriceDto>> GetAllAsync()
        {
            var cargoPrices = await _cargoPriceRepository.GetAllAsync();
            return cargoPrices.Adapt<List<ResultCargoPriceDto>>();
        }

        public async Task<UpdateCargoPriceDto> GetByIdAsync(Guid id)
        {
            var cargoPrice = await _cargoPriceRepository.GetByIdAsync(id);

            if (cargoPrice is null)
                throw new ValidationException("Fiyat tanımı bulunamadı.");

            return cargoPrice.Adapt<UpdateCargoPriceDto>();
        }

        public async Task UpdateAsync(UpdateCargoPriceDto updateCargoPriceDto)
        {
            var cargoPrice = updateCargoPriceDto.Adapt<CargoPrice>();
            await _cargoPriceRepository.UpdateAsync(cargoPrice);
        }
    }
}
