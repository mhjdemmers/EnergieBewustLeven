using EnergieBewustLeven.API.Models.DTO;
using EnergieBewustLeven.MVC.Data;
using EnergieBewustLeven.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class MeasurementsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;

        public MeasurementsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
        }

        //Hosted web API REST Service base url
        string Baseurl = "https://localhost:7218/api/";

        public IActionResult AdminIndex()
        {
            IEnumerable<MeasurementDTO> measurements = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl);

                //Making a HttpGet Request
                var responseTask = client.GetAsync("measurements");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<MeasurementDTO>>();
                    readTask.Wait();

                    measurements = readTask.Result;
                }
                else
                {
                    //Error response received   
                    measurements = Enumerable.Empty<MeasurementDTO>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(measurements);
        }
        
        public async Task<IActionResult> Create(Guid id)
        {
            // Call the API to get the list of ApplianceDTOs
            List<ApplianceDTO> applianceDTOs = await GetApplianceDTOsFromApiAsync();
            
            List<SelectListItem> selectListItems = GetApplianceIdList(applianceDTOs);

            // Populate the ViewBag with a list of SelectListItem using the ApplianceDTOs
            ViewBag.ApplianceIdList = selectListItems;

            if (id != null)
            {
                ViewBag.SelectedAppliance = selectListItems.FirstOrDefault(x => x.Value == id.ToString());
            }

            return View();
        }

        //POST: measurements/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddMeasurementRequestDTO measurement)
        {
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl);

                //Making a HttpPost Request
                var responseTask = client.PostAsJsonAsync("Measurements", measurement);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<AddMeasurementRequestDTO>();
                    readTask.Wait();

                    measurement = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

                AddLevel();

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Delete(Guid id)
        {
            MeasurementDTO measurement = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl + "measurements/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<MeasurementDTO>();
                    readTask.Wait();

                    measurement = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View(measurement);
        }

        private List<SelectListItem> GetApplianceIdList(List<ApplianceDTO> applianceDTOs)
        {
            // Create a list of SelectListItem from the ApplianceDTOs
            List<SelectListItem> selectListItems = applianceDTOs
                .Select(appliance => new SelectListItem { Value = appliance.Id.ToString(), Text = appliance.Name })
                .ToList();

            return selectListItems;
        }

        private async Task<List<ApplianceDTO>> GetApplianceDTOsFromApiAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                // Replace the base address with your actual API base address
                client.BaseAddress = new Uri(Baseurl);

                // Replace the endpoint with your actual API endpoint for getting appliances
                HttpResponseMessage response = await client.GetAsync("Appliances");

                if (response.IsSuccessStatusCode)
                {
                    // Read and deserialize the response content
                    var appliancesDTO = await response.Content.ReadAsAsync<List<ApplianceDTO>>();
                    return appliancesDTO;
                }
                else
                {
                    // Handle error, log, throw exception, etc.
                    // For now, we'll return an empty list in case of an error
                    return new List<ApplianceDTO>();
                }
            }
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

            return Ok();
        }
    }
}
