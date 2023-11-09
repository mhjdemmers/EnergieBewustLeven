using EnergieBewustLeven.MVC.Constants;
using EnergieBewustLeven.MVC.Data;
using EnergieBewustLeven.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
            _logger = logger;
        }
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NavScreen()
        {
            return View();
        }

        public IActionResult ListPlaceholder()
        {
            return View();
        }

        public IActionResult AddPlaceholder()
        {
            return View();
        }

        

        public ApplicationUser GetLoggedInUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser = dbContext.Users.FirstOrDefault(x => x.Id == userId);
            return applicationUser;
        }
        
        public IActionResult ProgressionPlaceholder()
        {

            var loggedInUser = GetLoggedInUser();
            return View(loggedInUser);
        }
        public IActionResult ProgressionNext()
        {
            return RedirectToAction("ProgressionPlaceholder");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}