using EnergieBewustLeven.API.Data;
using EnergieBewustLeven.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public ApplicationUser GetLoggedInUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            return applicationUser;
        }

        public IActionResult AddLevel()
        {
            var user = GetLoggedInUser();
            user.Level = user.Level + 1;
            dbContext.Update(user);
            dbContext.SaveChanges();
            return RedirectToAction("ProgressionPlaceholder", "Home");
        }

    }
}
