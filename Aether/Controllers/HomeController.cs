using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aether.Models;
using Newtonsoft.Json.Linq;

namespace Aether.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {            
            string zipCode = "49503";  // GR 49503 - KZOO 49001 - DETROIT 48127
            string key = APIKeys.AirNowAPI; // key hidden in APIKeys Model
            string dateTime = "2019-07-04T00-0000";

            // URL to get historic data via Zip Code
            string URL = $"http://www.airnowapi.org/aq/observation/zipCode/historical/?format=application/json&zipCode={zipCode}&date={dateTime}&distance=25&API_KEY={key}";

            // URL to get data via Zip Code
            //string URL = $"http://www.airnowapi.org/aq/observation/zipCode/current/?format=application/json&zipCode={zipCode}&distance=10&API_KEY={key}";

            JToken jt = ParseAPI.APICall(URL);
            List<AQIs> ListOfAQIs = new List<AQIs>();

            if (jt.Count() == 0)
            {
                ViewBag.Message = "Ooops. The API is Down.";
            }
            else
            {
                for (int i = 0; i < jt.Count(); i++)
                {
                    ListOfAQIs.Add(new AQIs(jt, i));
                }
            }

            // Find highest AQI
            int highestAQI = 0;
            foreach(AQIs a in ListOfAQIs)
            {
                if (a.AQI > highestAQI)
                {
                    highestAQI = a.AQI;
                }
            }

            // Get AQI Color
            int AQIIndex;
            if (highestAQI > 200)
            {
                AQIIndex = (highestAQI - 1) / 100 + 2;
            }
            else
            {
                AQIIndex = (highestAQI - 1) / 50;
            }

            // In case AQI gets over 400, set index to 5
            if (AQIIndex > 5)
            {
                AQIIndex = 5;
            }

            string colorAQI = returnHexColor(AQIIndex);

            ViewBag.highestAQI = highestAQI;
            ViewBag.AQIColor = colorAQI;
            ViewBag.AQIList = ListOfAQIs;

            return View();
        }


        public IActionResult About()
        {
            // METHOD FOR GETTING WEATHER FORECASTS
            string key = APIKeys.WeatherAPI;
            string cityCode = "4994358";
            string URL = $"http://api.openweathermap.org/data/2.5/forecast?id={cityCode}&APPID={key}";

            JToken jt = ParseAPI.APICall(URL);

            // Forecast readings are every 3h: 8=1 day, 24=3days, 39=5days minus 3h
            List<int> indexes = new List<int>() { 0, 8, 24, 39 };

            List<WeatherDataFromAPI> weatherForecast = new List<WeatherDataFromAPI>();

            foreach (int index in indexes)
            {
                WeatherDataFromAPI wd = new WeatherDataFromAPI(jt, index);
                wd.TemperatureC = wd.TemperatureK - 273.15;
                wd.TemperatureF = (wd.TemperatureC) * 9 / 5 + 32;

                weatherForecast.Add(wd);
            }

            ViewBag.WeatherForecast = weatherForecast;

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



        public static string returnHexColor(int index)
        {
            string[] hexColors = { "00e400", "ffff00", "ff7e00", "ff0000", "8f3f97", "7e0023" };
            //                      Green     Yellow    Orange    Red       Purple    Maroon
            return hexColors[index];

        }
    }
}
