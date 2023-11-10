namespace EnergieBewustLeven.API.Models.DTO
{
    public class AddReviewRequestDTO
    {
        public Guid ApplianceId { get; set; }
        public string? ReviewText { get; set; }
        public int ReviewScore { get; set; }
    }
}
