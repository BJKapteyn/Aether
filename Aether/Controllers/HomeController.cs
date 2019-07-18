using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aether.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Aether.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;


        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult AirQuality(string address)
        {
            //just put the default list in for now.
            if(address == "" || address == null)
            {
                return RedirectToAction("Test");
            }
            DisplayToUserInformation UserInfo = new DisplayToUserInformation();
            UserLatLng userLatLng = Geocode.UserLocation(address).Result;
            List<Sensor> sensors = Geocode.OrderedSensors(userLatLng);
            UserInfo.UserLatitude = userLatLng.Lat;
            UserInfo.UserLongitude = userLatLng.Lng;

            //List<Sensor> sensors = Sensor.GetSensors();
            Pollutants pollutantAQIs = new Pollutants();

            //going to change this to a loop later, use i to test various sensors.-----------------------------------
            int i = 0;

            UserInfo.SensorName = sensors[i].Name;
            PullSimsData pollutants1Hr = new PullSimsData(sensors[i], -1, configuration);
            pollutants1Hr.AddAQIs();
            UserInfo.AQINO2 = pollutants1Hr.NO2AQI;
            if (pollutants1Hr.NO2Average != null)
            { 
                UserInfo.NO2Avg = (double)pollutants1Hr.NO2Average;
            }
            else
            {
                UserInfo.NO2Avg = 0;
            }

            UserInfo.AQISO2 = pollutants1Hr.SO2AQI;

            PullSimsData pollutants8Hr = new PullSimsData(sensors[i], -8, configuration);
            pollutants8Hr.AddAQIs();
            UserInfo.AQICO = pollutants8Hr.COAQI;

            if (pollutants1Hr.COAverage != null)
            {
                UserInfo.COAvg = (double)pollutants8Hr.COAverage;
            }
            else
            {
                UserInfo.COAvg = 0;
            }

            if (pollutants1Hr.O3Average >= 0.125)
            {
                    UserInfo.AQIO3 = pollutants8Hr.O3AQI;
            }
            else
            {
                UserInfo.AQIO3 = pollutants1Hr.O3AQI;
            }

            PullSimsData pollutant24Hr = new PullSimsData(sensors[i], -24, configuration);
            pollutant24Hr.AddAQIs();

            UserInfo.AQIPM10 = pollutant24Hr.PM10AQI;
            UserInfo.AQIPM25 = pollutant24Hr.PM25AQI;

            List<FutureAQIs> futureAQIs = getFutureAQIs(UserInfo.O3Avg, UserInfo.COAvg, UserInfo.NO2Avg) ;
            UserInfo.FutureAQIs = futureAQIs; // sent to view as FutureAQIs object from DisplayToUserInformation model
                                              // 3x3 list index 0 = 1 day, index 1 = 3 day, index 2 = 5 day & .O3, .CO, .NO2
            UserInfo.CalculateHighestAQI();
            UserInfo.Sensor = sensors[0];

            return View(UserInfo);
        }

        public IActionResult Index()
        {
            List<AQIs> AQIList = APIController.GetHistoricAQIList();
            int highestAQI = getHighestAQI(AQIList);
            int AQIIndex = getAQIIndexPosition(highestAQI);

            ViewBag.highestAQI = highestAQI;
            ViewBag.AQIColor = returnHexColor(AQIIndex);
            ViewBag.AQIList = AQIList;

            return View();
        }


        public IActionResult About()
        {
            List<WeatherDataFromAPI> weatherForecast = APIController.GetWeatherForcast();

            ViewBag.WeatherForecast = weatherForecast;

            return View();
        }


        public IActionResult Contact()
        {

            return View();
        }

        public IActionResult EPA()
        {


            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    List<Sensor> sensors = Sensor.GetSensors();
        //    UserLatLng latLng = Geocode.UserLocation(address).Result;
        //    //ViewData.Model = latLng;
        //    return View(latLng);
        //}


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
                if (a.O3AQI > highestAQI) highestAQI = a.O3AQI;
            }

            return highestAQI;
        }

        public static int getAQIIndexPosition(int highestAQI)
        {
            int AQIIndex;

            if (highestAQI > 200)
            {
                AQIIndex = (highestAQI - 1) / 100 + 2;

                if (AQIIndex > 5) AQIIndex = 5;
            }
            else
            {
                AQIIndex = (highestAQI - 1) / 50;
            }

            return AQIIndex;

        }

        public static List<FutureAQIs> getFutureAQIs(double O3Average, double COAverage, double NO2Average)
        {
            List<WeatherDataFromAPI> weatherForecast = APIController.GetWeatherForcast();

            List<FutureAQIs> futureAQIs = new List<FutureAQIs>();

            for (int i = 1; i < 4; i++)
            {
                futureAQIs.Add(AQICalculations.AQIForecastEquation(weatherForecast, i, O3Average, COAverage, NO2Average));

            }

            return futureAQIs;
        }

    }

}