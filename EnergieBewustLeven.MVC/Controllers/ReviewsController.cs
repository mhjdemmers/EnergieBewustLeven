using EnergieBewustLeven.API.Models.DTO;
using EnergieBewustLeven.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext dbContext;

        public ReviewsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            this.dbContext = dbContext;
        }

        //Hosted web API REST Service base url
        string Baseurl = "https://localhost:7218/api/";

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

        public IActionResult AdminIndex()
        {
            IEnumerable<ReviewDTO> reviews = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl);

                //Making a HttpGet Request
                var responseTask = client.GetAsync("reviews");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ReviewDTO>>();
                    readTask.Wait();

                    reviews = readTask.Result;
                }
                else
                {
                    //Error response received   
                    reviews = Enumerable.Empty<ReviewDTO>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(reviews);
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

        //POST: reviews/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddReviewRequestDTO review)
        {
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl);

                //Making a HttpPost Request
                var responseTask = client.PostAsJsonAsync("Reviews", review);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<AddReviewRequestDTO>();
                    readTask.Wait();

                    review = readTask.Result;
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
            ReviewDTO review = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl + "reviews/ById/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ReviewDTO>();
                    readTask.Wait();

                    review = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
                return View(review);
            }
        }

        //DELETE: review/delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ReviewDTO review)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl + "Reviews/");

                var responseTask = client.DeleteAsync(review.Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error please try again after some time");
                }

                return RedirectToAction("Index", "Home");
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
