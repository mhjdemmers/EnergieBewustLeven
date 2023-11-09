using EnergieBewustLeven.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class MeasurementsController : Controller
    {

        //Hosted web API REST Service base url
        string Baseurl = "https://localhost:7218/api/";

        public IActionResult Create()
        {
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

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
