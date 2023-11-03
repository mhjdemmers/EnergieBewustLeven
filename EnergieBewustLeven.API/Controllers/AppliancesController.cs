using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EnergieBewustLeven.API.Models.Domain;
using EnergieBewustLeven.API.Data;
using EnergieBewustLeven.API.Models.DTO;

namespace EnergieBewustLeven.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppliancesController : ControllerBase
    {
        private readonly EnergieBewustLevenDbContext dbContext;

        public AppliancesController(EnergieBewustLevenDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET ALL APPLIANCES
        //GET: 
        [HttpGet]
        public IActionResult GetAppliances()
        {
            // Get from database - Domain Model
            var appliancesDomain = dbContext.Appliances.ToList();

            // Map Domain Models to DTOs
            var appliancesDTO = new List<ApplianceDTO>();
            foreach (var applianceDomain in appliancesDomain)
            {
                appliancesDTO.Add(new ApplianceDTO()
                {
                    Id = applianceDomain.Id,
                    Name = applianceDomain.Name,
                    Brand = applianceDomain.Brand
                });
            }

            // Return DTOs
            return Ok(appliancesDTO);
        }

        //GET APPLIANCE BY ID
        //GET:
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetAppliance([FromRoute] Guid id)
        {
            // Get from database - Domain Model
            var applianceDomain = dbContext.Appliances.FirstOrDefault(x => x.Id == id);

            if (applianceDomain == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            var applianceDTO = new ApplianceDTO()
            {
                Id = applianceDomain.Id,
                Name = applianceDomain.Name,
                Brand = applianceDomain.Brand
            };

            // Return DTO
            return Ok(applianceDTO);
        }

        // POST CREATE NEW APPLIANCE
        // POST:
        [HttpPost]
        public IActionResult CreateAppliance([FromBody] AddApplianceRequestDTO addApplianceRequestDTO)
        {
            // Map DTO to Domain Model
            var applianceDomain = new Appliance
            {
                Name = addApplianceRequestDTO.Name,
                Brand = addApplianceRequestDTO.Brand
            };

            // Use Domain Model to create appliance
            dbContext.Appliances.Add(applianceDomain);
            dbContext.SaveChanges();

            // Map Domain back to DTO
            var applianceDTO = new ApplianceDTO()
            {
                Id = applianceDomain.Id,
                Name = applianceDomain.Name,
                Brand = applianceDomain.Brand
            };

            return CreatedAtAction(nameof(GetAppliance), new { id = applianceDomain.Id }, applianceDTO);
        }

        // PUT UPDATE APPLIANCE
        // PUT:
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult UpdateAppliance([FromRoute] Guid id, [FromBody] UpdateApplianceRequestDTO updateApplianceRequestDTO)
        {
            // Get from database - Domain Model
            var applianceDomain = dbContext.Appliances.FirstOrDefault(x => x.Id == id);

            if (applianceDomain == null)
            {
                return NotFound();
            }

            applianceDomain.Name = updateApplianceRequestDTO.Name;
            applianceDomain.Brand = updateApplianceRequestDTO.Brand;

            dbContext.SaveChanges();

            return Ok();



        }

        // DELETE APPLIANCE
        // DELETE:
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteAppliance([FromRoute] Guid id)
        {
            // Get from database - Domain Model
            var applianceDomain = dbContext.Appliances.FirstOrDefault(x => x.Id == id);

            if (applianceDomain == null)
            {
                return NotFound();
            }

            // Delete from database
            dbContext.Appliances.Remove(applianceDomain);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
