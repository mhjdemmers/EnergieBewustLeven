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
        public IActionResult GetAll()
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
        public IActionResult Get([FromRoute] Guid id)
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
    }
}
