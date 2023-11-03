namespace EnergieBewustLeven.API.Models.DTO
{
    public class MeasurementDTO
    {
        public Guid Id { get; set; }
        public Guid ApplianceId { get; set; }
        public int Verbruik { get; set; }
    }
}
