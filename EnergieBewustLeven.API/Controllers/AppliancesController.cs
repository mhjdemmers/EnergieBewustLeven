using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnergieBewustLeven.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppliancesController : ControllerBase
    {
        // GET: 
        [HttpGet]
        public IActionResult GetAllAppliances()
        {
            string[] appliancesNames = new string[] { "yeet", "Jane", "Mark"};
            return Ok(appliancesNames);
        }
    }
}
