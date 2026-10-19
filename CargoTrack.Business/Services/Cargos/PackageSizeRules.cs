using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Services.Cargos
{
    public class PackageSizeRules
    {
        public static double GetBillableDesi(PackageSize size) => size switch
        {
            PackageSize.Micro => 3,
            PackageSize.Small => 6,
            PackageSize.Large => 10,
            PackageSize.Voluminous => 30,
            _ => throw new ArgumentOutOfRangeException(nameof(size), "Geçersiz paket boyutu.")
        };

        public static string GetDisplayName(PackageSize size) => size switch
        {
            PackageSize.Micro => "Mikro / Dosya (3 desiye kadar)",
            PackageSize.Small => "Küçük (6 desiye kadar)",
            PackageSize.Large => "Büyük (10 desiye kadar)",
            PackageSize.Voluminous => "Hacimli (30 desiye kadar)",
            _ => size.ToString()
        };
    }
}
