using EnergieBewustLeven.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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

        public IActionResult ProgressionPlaceholder()
        {
            return View();
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