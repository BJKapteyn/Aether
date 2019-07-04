using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aether.Models;

namespace Aether.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult AirQuality()
        {
            DisplayToUserInformation rv = new DisplayToUserInformation();

            //rv.O3AQI = Math.Round(PollutantController.CalculateAQI(CalculationController.pollutantAverages[0], pm25BreakpointIndex, 3));
            //rv.PM25AQI = Math.Round(PollutantController.CalculateEPA(PollutantController.PollutantAverages[2], pm25BreakpointIndex, 3));
            //rv.PM25AQI = Math.Round(PollutantController.CalculateEPA(PollutantController.PollutantAverages[4], pm25BreakpointIndex, 3));
            //rv.NO2AQI = Math.Round(PollutantController.CalculateEPA(PollutantController.PollutantAverages[3], no2BreakpointIndex, 6));
            //rv.SO2AQI = Math.Round(PollutantController.CalculateEPA(PollutantController.PollutantAverages[5], so2BreakpointIndex, 5));
            //rv.CO = Math.Round(PollutantController.CalculateEPA(PollutantController.PollutantAverages[2], coBreakpointIndex, 4));

            //rv.PredictedAQITomorrow = Math.Round(ForecastedAQI[1]);
            //rv.PredictedAQI3Day = Math.Round(ForecastedAQI[2]);
            //rv.PredictedAQI5Day = Math.Round(ForecastedAQI[3]);

            return View(rv);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
