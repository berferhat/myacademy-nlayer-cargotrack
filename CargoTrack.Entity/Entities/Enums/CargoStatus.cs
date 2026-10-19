

namespace CargoTrack.Entity.Entities.Enums
{
    public enum CargoStatus
    {
        Created = 1,                    // Oluşturuldu
        AtOriginBranch = 2,             // Gönderici şubesinde
        AtTransferCenter = 3,           // Transfer merkezinde
        AtDestinationBranch = 4,        // Varış şubesinde
        OutForDelivery = 5,             // Dağıtıma çıktı
        Delivered = 6,                  // Teslim edildi
        
        DeliveryFailed = 7,             // Teslim edilemedi
        InReturnProcess = 8,            // İade sürecinde
        ReturnedToSender = 9            // Göndericiye iade edildi
    }
}
