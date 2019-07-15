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
            //List<Sensor> sensors = Geocode.OrderedSensors(address);
            List<Sensor> sensors = Sensor.GetSensors();
            Pollutants pollutantAQIs = new Pollutants();
            DisplayToUserInformation UserInfo = new DisplayToUserInformation();

            //going to change this to a loop later, use i to test various sensors.
            int i = 6;
            if (sensors[i].Name.Contains("graq"))
            {
                //refactor this with linq and just pull 24hrs once and add all of the readings to one object--------
                PullOSTData oneHrData = new PullOSTData(sensors[i], -1, configuration);
                //grab the list of pollutantdata and generate aqis based on the hour
                Pollutants pollutants1hr = new Pollutants(oneHrData.Data);

                PullOSTData eightHrData = new PullOSTData(sensors[i], -8, configuration);
                Pollutants pollutants8Hr = new Pollutants(eightHrData.Data);

                if(pollutants1hr.O3Average >= 0.125)
                {
                    UserInfo.AQIO3 = pollutants8Hr.O3AQI;
                }
                else
                {
                    UserInfo.AQIO3 = pollutants1hr.O3AQI;
                }

                PullOSTData fullDayData = new PullOSTData(sensors[i], -24, configuration);
                Pollutants pollutants24hr = new Pollutants(fullDayData.Data);

                UserInfo.AQIPM10 = pollutants24hr.PM10AQI;
                UserInfo.AQIPM25 = pollutants24hr.PM25AQI;

            }
            else
            {
                PullSimsData oneHrData = new PullSimsData(sensors[i], -1, configuration);
                Pollutants pollutants1Hr = new Pollutants(oneHrData.Data);
                UserInfo.AQINO2 = pollutants1Hr.NO2AQI;
                UserInfo.AQISO2 = pollutants1Hr.SO2AQI;

                PullSimsData eightHrData = new PullSimsData(sensors[i], -8, configuration);
                Pollutants pollutants8Hr = new Pollutants(eightHrData.Data);
                if(pollutants1Hr.O3Average >= 0.125)
                {
                    UserInfo.AQIO3 = pollutants8Hr.O3AQI;
                }
                else
                {
                    UserInfo.AQIO3 = pollutants1Hr.O3AQI;
                }

                PullSimsData fullDayData = new PullSimsData(sensors[i], -24, configuration);
                Pollutants pollutant24Hr = new Pollutants(fullDayData.Data);

            }
            List<double> futureAQIs = getFutureAQIs(UserInfo.AQIO3);
            UserInfo.AQIPredictedTomorrow = futureAQIs[1];
            UserInfo.AQIPredicted3Day = futureAQIs[2];
            UserInfo.AQIPredicted5Day = futureAQIs[3];

            return View(UserInfo);
        }

        public IActionResult Index()
        {
            //List<AQIs> AQIList = APIController.GetListAQI("48127");
            //int highestAQI = getHighestAQI(AQIList);
            ////int AQIIndex = getAQIIndexPosition(highestAQI);

            //ViewBag.highestAQI = highestAQI;
            //ViewBag.AQIColor = returnHexColor(AQIIndex);
            //ViewBag.AQIList = AQIList;

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

        public IActionResult Test()
        {
            List<Sensor> sensors = Sensor.GetSensors();
            PullOSTData pullData = new PullOSTData();
            List<PollutantData> data = pullData.PullData(sensors[0], 8, configuration);

            return View(data);
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
                if (a.AQI > highestAQI) highestAQI = a.AQI;
            }

            return highestAQI;
        }


        //public static int getAQIIndexPosition(int highestAQI)
        //{
        //    int AQIIndex;

        //    if (highestAQI > 200)
        //    {       
        //        AQIIndex = (highestAQI - 1) / 100 + 2;

        //        if (AQIIndex > 5) AQIIndex = 5;
        //    }
        //    else
        //    {
        //        AQIIndex = (highestAQI - 1) / 50;
        //    }

        //    return AQIIndex;

        //}


        public static List<double> getFutureAQIs(double O3Reading)
        {
            List<WeatherDataFromAPI> weatherForecast = APIController.GetWeatherForcast();

            List<double> futureAQIs = new List<double>();

            for (int i = 0; i < 4; i++)
            {
                futureAQIs.Add(AQICalculations.WeatherForecastEquation(weatherForecast, i, O3Reading));
            }

            return futureAQIs;
        }
    }
}