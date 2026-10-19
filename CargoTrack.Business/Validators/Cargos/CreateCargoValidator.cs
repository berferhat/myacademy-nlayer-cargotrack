using CargoTrack.DTO.DTOs.CargoDtos;
using FluentValidation;

namespace CargoTrack.Business.Validators.Cargos
{
    public class CreateCargoValidator : AbstractValidator<CreateCargoDto>
    {
        public CreateCargoValidator()
        {
            RuleFor(x => x.SenderId).NotEmpty().WithMessage("Gönderici seçilmelidir.");
            RuleFor(x => x.ReceiverId).NotEmpty().WithMessage("Alıcı seçilmelidir.")
                .NotEqual(x => x.SenderId).WithMessage("Gönderici ve alıcı aynı olamaz.");
            RuleFor(x => x.OriginBranchId).NotEmpty().WithMessage("Çıkış şubesi seçilmelidir.");
            RuleFor(x => x.DestinationBranchId).NotEmpty().WithMessage("Varış şubesi seçilmelidir.")
                .NotEqual(x => x.OriginBranchId).WithMessage("Çıkış ve varış şubesi aynı olamaz.");
            RuleFor(x => x.Weight).GreaterThan(0).WithMessage("Ağırlık 0'dan büyük olmalıdır.");
            RuleFor(x => x.PackageSize).IsInEnum().WithMessage("Geçerli bir paket boyutu seçilmelidir.");
            RuleFor(x => x.CargoType).IsInEnum().WithMessage("Geçerli bir kargo tipi seçilmelidir.");
        }
    }
}
