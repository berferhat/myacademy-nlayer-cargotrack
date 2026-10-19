using CargoTrack.DTO.DTOs.CargoDtos;
using CargoTrack.Entity.Entities.Enums;
using FluentValidation;

namespace CargoTrack.Business.Validators.Cargos
{
    public class ChangeCargoStatusValidator : AbstractValidator<ChangeCargoStatusDto>
    {
        public ChangeCargoStatusValidator()
        {
            RuleFor(x => x.CargoId).NotEmpty().WithMessage("Kargo seçilmelidir.");
            RuleFor(x => x.NewStatus).IsInEnum().WithMessage("Geçerli bir durum seçilmelidir.");
            When(x => x.NewStatus == CargoStatus.AtTransferCenter, () =>
            {
                RuleFor(x => x.TransferCenterId)
                    .NotEmpty().WithMessage("Transfer merkezi seçilmelidir.");
            });
            When(x => x.NewStatus == CargoStatus.DeliveryFailed, () =>
            {
                RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Teslim edilememe nedeni zorunludur.");
            });
            When(x => x.NewStatus == CargoStatus.Delivered, () =>
            {
                RuleFor(x => x.DeliveryCode)
                    .NotEmpty().WithMessage("Teslimat kodu girilmelidir.");
                RuleFor(x => x.ReceivedByName)
                    .NotEmpty().WithMessage("Teslim alan kişi adı zorunludur.");
            });
        }
    }
}