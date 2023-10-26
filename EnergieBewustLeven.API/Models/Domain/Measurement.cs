namespace EnergieBewustLeven.API.Models.Domain
{
    public class Measurement
    {
        public Guid Id { get; set; }
        public Guid ApplianceId { get; set; }
        public int Verbruik { get; set; }

    }
}
