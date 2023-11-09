using EnergieBewustLeven.API.Models.DTO;

namespace EnergieBewustLeven.MVC.Models
{
    public class ApplianceDetailViewModel
    {
        public Guid ApplianceId { get; set; }
        public string? ApplianceName { get; set; }
        public string? ApplianceBrand { get; set; }
        public List<MeasurementDTO>? Measurements { get; set; }
        public List<ReviewDTO>? Reviews { get; set; }
    }
}
