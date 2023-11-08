using EnergieBewustLeven.API.Data;
using EnergieBewustLeven.API.Models.Domain;
using EnergieBewustLeven.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnergieBewustLeven.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly EnergieBewustLevenDbContext dbContext;

        public MeasurementsController(EnergieBewustLevenDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET ALL MEASUREMENT
        //GET: 
        [HttpGet]
        public IActionResult Measurements()
        {
            // Get from database - Domain Model
            var measurementsDomain = dbContext.Measurements.ToList();

            // Map Domain Models to DTOs
            var measurementsDTO = new List<MeasurementDTO>();
            foreach (var measurementDomain in measurementsDomain)
            {
                measurementsDTO.Add(new MeasurementDTO()
                {
                    Id = measurementDomain.Id,
                    ApplianceId = measurementDomain.ApplianceId,
                    Verbruik = measurementDomain.Verbruik
                });
            }

            // Return DTOs
            return Ok(measurementsDTO);
        }

        //GET MEASUREMENT BY ID
        //GET:
        [HttpGet]
        [Route("ById/{id:Guid}")]
        public IActionResult GetMeasurement([FromRoute] Guid id)
        {
            // Get from database - Domain Model
            var measurementDomain = dbContext.Measurements.FirstOrDefault(x => x.Id == id);

            if (measurementDomain == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            var measurementDTO = new MeasurementDTO()
            {
                Id = measurementDomain.Id,
                ApplianceId = measurementDomain.ApplianceId,
                Verbruik = measurementDomain.Verbruik
            };

            // Return DTO
            return Ok(measurementDTO);
        }

        //GET MEASUREMENT BY APPLIANCE
        //GET:
        [HttpGet]
        [Route("ByAppliance/{id:Guid}")]
        public async Task<IActionResult> GetMeasurementsByAppliance([FromRoute] Guid applianceId)
        {
            // Get from database - Domain Model
            var measurementsDomain = await dbContext.Measurements.Where(m => m.ApplianceId == applianceId).ToListAsync();

            // Map Domain Models to DTOs
            var measurementsDTO = new List<MeasurementDTO>();
            foreach (var measurementDomain in measurementsDomain)
            {
                measurementsDTO.Add(new MeasurementDTO()
                {
                    Id = measurementDomain.Id,
                    ApplianceId = measurementDomain.ApplianceId,
                    Verbruik = measurementDomain.Verbruik
                });
            }

            // Return DTOs
            return Ok(measurementsDTO);
        }

        // POST CREATE NEW MEASUREMENT
        // POST:
        [HttpPost]
        public IActionResult CreateMeasurement([FromBody] AddMeasurementRequestDTO addMeasurementRequestDTO)
        {
            // Map DTO to Domain Model
            var measurementDomain = new Measurement
            {
                ApplianceId = addMeasurementRequestDTO.ApplianceId,
                Verbruik = addMeasurementRequestDTO.Verbruik
            };

            // Use Domain Model to create appliance
            dbContext.Measurements.Add(measurementDomain);
            dbContext.SaveChanges();

            // Map Domain back to DTO
            var measurementDTO = new MeasurementDTO()
            {
                Id = measurementDomain.Id,
                ApplianceId = addMeasurementRequestDTO.ApplianceId,
                Verbruik = addMeasurementRequestDTO.Verbruik
            };

            return CreatedAtAction(nameof(GetMeasurement), new { id = measurementDomain.Id }, measurementDTO);
        }

        // DELETE MEASUREMENT
        // DELETE:
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteMeasurement([FromRoute] Guid id)
        {
            // Get from database - Domain Model
            var measurementDomain = dbContext.Measurements.FirstOrDefault(x => x.Id == id);

            if (measurementDomain == null)
            {
                return NotFound();
            }

            // Delete from database
            dbContext.Measurements.Remove(measurementDomain);
            dbContext.SaveChanges();

            return NoContent();
        }

        
    }
}
