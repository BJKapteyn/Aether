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
            // FROM OLD METHOD OF CALLING API BY AREA AND THIS ALSO REQUIRED A TIME
            //DateTime nowHour = DateTime.Now.AddHours(4); // to add 4 hours to UTC for EDT
            //string currentTime = nowHour.ToString("yyyy-MM-ddTHH");

            // DETROIT 48127
            // KZOO 49001
            // GR 49503
            string zipCode = "49503";
            string key = APIKeys.AirNowAPI; // key hidden in APIKeys Model

            // URL to get area with historic data via datetime
            //string URL = $"http://www.airnowapi.org/aq/data/?startDate=2019-07-04T07&endDate=2019-07-04T07&parameters=OZONE,PM25,PM10,CO,NO2,SO2&BBOX=-85.826092,42.825368,-85.386639,43.082676&dataType=A&format=application/json&verbose=0&nowcastonly=0&API_KEY={key}";

            // URL to get data via Zip Code
            string URL = $"http://www.airnowapi.org/aq/observation/zipCode/current/?format=application/json&zipCode={zipCode}&distance=10&API_KEY={key}";

            JToken jt = ParseAPI.APICall(URL);
            List<AQIs> ListOfAQIs = new List<AQIs>();

            if (jt == null)
            {
                ViewBag.Message = "Ooops. That didn't work.";
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



        public static string returnHexColor(int index)
        {
            string[] hexColors = { "00e400", "ffff00", "ff7e00", "ff0000", "8f3f97", "7e0023" };
            //                      Green     Yellow    Orange    Red       Purple    Maroon
            return hexColors[index];

            //if (highestAQI <= 50)
            //{
            //    return "limegreen"; // GREEN 00e400 0
            //}
            //else if (highestAQI > 50 && highestAQI <= 100)
            //{
            //    return "yellow"; // YELLOW ffff00 1
            //}
            //else if (highestAQI > 100 && highestAQI <= 150)
            //{
            //    return "orange"; // ORANGE ff7e00 2
            //}
            //else if (highestAQI > 150 && highestAQI <= 200)
            //{
            //    return "red"; // RED ff0000 3
            //}
            //else if (highestAQI > 200 && highestAQI <= 300)
            //{
            //    return "purple"; // PURPLE 8f3f97 4
            //}
            //else
            //{
            //    return "maroon"; // MAROON 7e0023 5
        }
    }

    //public static List<WeatherDataFromAPI> WeatherData()
    //{
    //    //bringing in windspeed, temperature JTokens etc. from the API 
    //    JToken weather = WeatherAPIDAL.Json();

    //    // Forecast (with an 'e') readings are every 3h: 8=1 day, 24=3days, 39=5days minus 3h
    //    List<int> indexes = new List<int>() { 0, 8, 24, 39 };

    //    List<WeatherDataFromAPI> weatherTime = new List<WeatherDataFromAPI>();

    //    //converting temperature from Kelvin to Celsius and Fahrenheit
    //    foreach (int index in indexes)
    //    {
    //        WeatherDataFromAPI wd = new WeatherDataFromAPI(weather, index);
    //        wd.TemperatureC = wd.TemperatureK - 273.15;
    //        wd.TemperatureF = (wd.TemperatureC) * 9 / 5 + 32;

    //        weatherTime.Add(wd);
    //    }

    //    return weatherTime;
    //}


}
