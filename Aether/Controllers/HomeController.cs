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
            List<AQIs> AQIList = APIController.GetListAQI();
            int highestAQI = getHighestAQI(AQIList);
            int AQIIndex = getAQIIndexPosition(highestAQI);

            ViewBag.highestAQI = highestAQI;
            ViewBag.AQIColor = returnHexColor(AQIIndex);
            ViewBag.AQIList = AQIList;

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


        public static int getHighestAQI(List<AQIs> AQIList)
        {
            int highestAQI = 0;

            foreach (AQIs a in AQIList)
            {
                if (a.AQI > highestAQI)
                {
                    highestAQI = a.AQI;
                }
            }

            return highestAQI;
        }


        public static int getAQIIndexPosition(int highestAQI)
        {
            int AQIIndex;

            if (highestAQI > 200)
            {       
                AQIIndex = (highestAQI - 1) / 100 + 2;
                if (AQIIndex > 5)
                {
                    AQIIndex = 5;
                }
            }
            else
            {
                AQIIndex = (highestAQI - 1) / 50;
            }

            return AQIIndex;

        }


        public static double WeatherForecastEquation(List<WeatherDataFromAPI> weatherTime, int index, double eightHourO3)
        {
            double FutureAQI1Day = (double)(5.3 * weatherTime[index].WindSpeed) + (double)(0.4 * weatherTime[index].TemperatureC) +
                (double)(0.1 * weatherTime[index].Humidity) + ((double)0.7 * eightHourO3);

            return FutureAQI1Day;

        }
    }
}