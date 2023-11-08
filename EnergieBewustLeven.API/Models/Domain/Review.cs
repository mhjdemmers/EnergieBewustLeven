namespace EnergieBewustLeven.API.Models.Domain
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ApplianceId { get; set; }
        public string? ReviewText { get; set; }
        public int ReviewScore { get; set; }
    }
}
