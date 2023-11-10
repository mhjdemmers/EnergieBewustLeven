namespace EnergieBewustLeven.API.Models.DTO
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }
        public Guid ApplianceId { get; set; }
        public string? ReviewText { get; set; }
        public int ReviewScore { get; set; }
    }
}
