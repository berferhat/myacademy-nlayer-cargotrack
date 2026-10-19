namespace CargoTrack.DTO.DTOs.AddressDtos
{
    public class ResultAddressDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public Guid UserId { get; set; }
    }
}
