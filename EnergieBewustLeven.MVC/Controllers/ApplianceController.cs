﻿using EnergieBewustLeven.API.Models.DTO;
using EnergieBewustLeven.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnergieBewustLeven.MVC.Controllers
{
    public class ApplianceController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<ApplianceDTO> appliances = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri("https://localhost:7218/api/");

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

        public IActionResult Details(Guid id)
        {
            ApplianceDTO appliance = null;
            using (var client = new HttpClient())
            {
                //Setting the base address of the API
                client.BaseAddress = new Uri("https://localhost:7218/api/appliances/");

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

            ApplianceDetailViewModel viewmodel = new ApplianceDetailViewModel();
            viewmodel.ApplianceId = appliance.Id;
            viewmodel.ApplianceName = appliance.Name;
            viewmodel.ApplianceBrand = appliance.Brand;
            viewmodel.Measurements = measurements;
            return View(viewmodel);
        }
    }
}