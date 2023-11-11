using EnergieBewustLeven.API.Models.DTO;
using EnergieBewustLeven.MVC.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Compare()
        {
            return View();
        }

        //public IActionResult TestAction()
        //{
        //    Guid id = new Guid();
        //    return RedirectToAction(Details(id).ToString());
        //}

        //public IActionResult Compare(CompareViewModel compareViewModel)
        //{

        //}
    }
}
