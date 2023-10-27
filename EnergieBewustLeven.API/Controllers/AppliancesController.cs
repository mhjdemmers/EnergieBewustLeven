using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EnergieBewustLeven.API.Models.Domain;
using EnergieBewustLeven.API.Data;

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
        // GET:
        [HttpGet]
        public IActionResult GetAll()
        {
            var appliances = dbContext.Appliances.ToList();

            return Ok(appliances);
        }
    }
}
