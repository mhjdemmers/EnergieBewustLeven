namespace EnergieBewustLeven.API.Models.DTO
{
    public class AddMeasurementRequestDTO
    {
        public Guid ApplianceId { get; set; }
        public int Verbruik { get; set; }
    }
}
