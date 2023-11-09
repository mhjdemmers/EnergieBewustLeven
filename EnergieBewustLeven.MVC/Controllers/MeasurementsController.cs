using EnergieBewustLeven.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class MeasurementsController : Controller
    {

        //Hosted web API REST Service base url
        string Baseurl = "https://localhost:7218/api/";

        

        public async Task<IActionResult> Create()
        {
            // Call the API to get the list of ApplianceDTOs
            List<ApplianceDTO> applianceDTOs = await GetApplianceDTOsFromApiAsync();

            // Populate the ViewBag with a list of SelectListItem using the ApplianceDTOs
            ViewBag.ApplianceIdList = GetApplianceIdList(applianceDTOs);

            return View();
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

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
