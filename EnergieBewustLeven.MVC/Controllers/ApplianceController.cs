using EnergieBewustLeven.API.Models.DTO;
using EnergieBewustLeven.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class ApplianceController : Controller
    {

        //Hosted web API REST Service base url
        string Baseurl = "https://localhost:7218/api/";

        public IActionResult Index()
        {
            IEnumerable<ApplianceDTO> appliances = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl);

                //Making a HttpGet Request
                var responseTask = client.GetAsync("appliances");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ApplianceDTO>>();
                    readTask.Wait();

                    appliances = readTask.Result;
                }
                else
                {
                    //Error response received   
                    appliances = Enumerable.Empty<ApplianceDTO>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(appliances);
        }

        public IActionResult Search(string searchString)
        {
            IEnumerable<ApplianceDTO> appliances = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                var responseTask = client.GetAsync("appliances");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ApplianceDTO>>();
                    readTask.Wait();

                    appliances = readTask.Result;

                    // Filter op naam indien zoektekst aanwezig is
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        appliances = appliances.Where(a => a.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
                    }
                }
                else
                {
                    appliances = Enumerable.Empty<ApplianceDTO>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View("Index", appliances);
        }

        public IActionResult AdminIndex()
        {
            IEnumerable<ApplianceDTO> appliances = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl);

                //Making a HttpGet Request
                var responseTask = client.GetAsync("appliances");
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ApplianceDTO>>();
                    readTask.Wait();

                    appliances = readTask.Result;
                }
                else
                {
                    //Error response received   
                    appliances = Enumerable.Empty<ApplianceDTO>();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            return View(appliances);
        }

        public IActionResult Create()
        {
            return View();
        }

        //POST: appliance/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AddApplianceRequestDTO appliance)
        {
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl);

                //Making a HttpPost Request
                var responseTask = client.PostAsJsonAsync("Appliances", appliance);
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<AddApplianceRequestDTO>();
                    readTask.Wait();

                    appliance = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Delete(Guid id)
        {
            ApplianceDTO appliance = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl + "appliances/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ApplianceDTO>();
                    readTask.Wait();

                    appliance = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            List<MeasurementDTO> measurements = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri("https://localhost:7218/api/Measurements/ByAppliance/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<MeasurementDTO>>();
                    readTask.Wait();

                    measurements = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            List<ReviewDTO> reviews = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri("https://localhost:7218/api/Reviews/ByAppliance/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ReviewDTO>>();
                    readTask.Wait();

                    reviews = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            ApplianceDetailViewModel viewmodel = new ApplianceDetailViewModel();
            viewmodel.ApplianceId = appliance.Id;
            viewmodel.ApplianceName = appliance.Name;
            viewmodel.ApplianceBrand = appliance.Brand;
            viewmodel.Measurements = measurements;
            viewmodel.Reviews = reviews;
            return View(viewmodel);
        }

        //DELETE: appliance/delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApplianceDTO appliance)
        {
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl + "Appliances/");

                var responseTask = client.DeleteAsync(appliance.Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;

                if(result.IsSuccessStatusCode)
                {
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error please try again after some time");
                }

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Details(Guid id)
        {
            ApplianceDTO appliance = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl + "appliances/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ApplianceDTO>();
                    readTask.Wait();

                    appliance = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            List<MeasurementDTO> measurements = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri("https://localhost:7218/api/Measurements/ByAppliance/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<MeasurementDTO>>();
                    readTask.Wait();

                    measurements = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            List<ReviewDTO> reviews = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri("https://localhost:7218/api/Reviews/ByAppliance/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(id.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<ReviewDTO>>();
                    readTask.Wait();

                    reviews = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            ApplianceDetailViewModel viewmodel = new ApplianceDetailViewModel();
            viewmodel.ApplianceId = appliance.Id;
            viewmodel.ApplianceName = appliance.Name;
            viewmodel.ApplianceBrand = appliance.Brand;
            viewmodel.Measurements = measurements;
            viewmodel.Reviews = reviews;
            return View(viewmodel);
        }

        public async Task<IActionResult> Compare()
        {
            // Call the API to get the list of ApplianceDTOs
            List<ApplianceDTO> applianceDTOs = await GetApplianceDTOsFromApiAsync();
            List<SelectListItem> selectListItems = GetApplianceIdList(applianceDTOs);

            // Populate the ViewBag with a list of SelectListItem using the ApplianceDTOs
            ViewBag.ApplianceIdList = selectListItems;

            return View();
        }

        public IActionResult CompareResults(Guid applianceId1, Guid applianceId2)
        {
            // Retrieve information about the appliances based on their IDs
            ApplianceDTO appliance1 = null;
            ApplianceDTO appliance2 = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri(Baseurl + "appliances/");

                //Making a HttpGet Request
                var responseTask = client.GetAsync(applianceId1.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                var result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ApplianceDTO>();
                    readTask.Wait();

                    appliance1 = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }

                //Making a HttpGet Request
                responseTask = client.GetAsync(applianceId2.ToString());
                responseTask.Wait();

                //To store result of web api response.   
                result = responseTask.Result;

                //If success received   
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ApplianceDTO>();
                    readTask.Wait();

                    appliance2 = readTask.Result;
                }
                else
                {
                    //Error response received   
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            // Create a new model to pass the details to the view
            var model = new CompareResultsViewModel
            {
                Appliance1 = appliance1,
                Appliance2 = appliance2
            };

            return View(model);
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
                client.BaseAddress = new Uri(Baseurl);

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
                    return new List<ApplianceDTO>();
                }
            }
        }

        //public IActionResult TestAction()
        //{
        //    Guid id = new Guid();
        //    return RedirectToAction(Details(id).ToString());
        //}
    }
}
