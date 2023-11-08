using EnergieBewustLeven.API.Data;
using EnergieBewustLeven.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;

        public UsersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
        }

        public int GetLevelUser(ApplicationUser user)
        {
            int level = user.Level;
            return level;
        }

        public IActionResult AddLevel(ApplicationUser user)
        {
            user.Level += 1;
            return Ok(user);
        }

    }
}
