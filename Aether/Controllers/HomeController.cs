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
        public static List<PollutantData> pollutantData1Hr = new List<PollutantData>();
        //public static List<PollutantData8Hr> pollutantData8Hr = new List<PollutantData8Hr>();
        //public static List<PollutantData24Hr> pollutantData24Hr = new List<PollutantData24Hr>();
        private readonly IConfiguration configuration;


        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult AirQuality(string address)
        {
            //just put the default list in for now.
            List<Sensor> sensors = Geocode.OrderedSensors(address);
            //Pull1hrData(sensors[0]);
            //Pull8hrData(sensors[0]);
            //Pull24hrData(sensors[0]);
            
            //AQICalculations.SumAndAveragePollutantReadings(sensors[0]);

            AQICalculations.BreakPointIndex();

            AQICalculations.AQI();

            DisplayToUserInformation rv = new DisplayToUserInformation
            {

                AQIO3 = AQICalculations.pollutantAQIs[0],
                AQIPM10 = AQICalculations.pollutantAQIs[1],
                AQIPM25 = AQICalculations.pollutantAQIs[2],
                AQICO = AQICalculations.pollutantAQIs[3],
                AQISO2 = AQICalculations.pollutantAQIs[4],
                AQINO2 = AQICalculations.pollutantAQIs[5],
                AQIToday = AQICalculations.MaxAQI()

                //AQISecondO3
                //AQIThirdO3
                //AQIPredictedTomorrow
                //AQIPredicted3Day
                //AQIPredicted5Day 
            };

            List<double> futureAQIs = getFutureAQIs(AQICalculations.pollutantAverages[0]);
            rv.AQIPredictedTomorrow = futureAQIs[0];
            rv.AQIPredicted3Day = futureAQIs[1];
            rv.AQIPredicted5Day = futureAQIs[2];

            return View(rv);
        }



        //public void Pull8hrData(Sensor s)
        //{
        //        DateTime nowDay = DateTime.Now;
        //        string currentHour = nowDay.ToString("HH:MM");
        //        DateTime pastHrs = nowDay.AddHours(-8);
        //        string pastTime = pastHrs.ToString("HH:MM");

        //        //pulls closest sensor name
        //        string sensorLocation = s.Name;
        //        string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");
        //        SqlConnection connection = new SqlConnection(connectionstring);

        //        connection.Open();

        //        string sql;

        //        if (sensorLocation.Contains("graq"))
        //        {
        //            sql = $"EXEC OSTSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
        //        }
        //        else
        //        {
        //            sql = $"EXEC SimmsSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
        //        }

        //        SqlCommand com = new SqlCommand(sql, connection);
        //        SqlDataReader rdr = com.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            if (sensorLocation.Contains("graq"))
        //            {
        //                var pollutant = new PollutantData8Hr
        //                {
        //                    Dev_id = (string)rdr["dev_id"],
        //                    Time = (DateTime)rdr["time"],
        //                    O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
        //                    Id = (int)rdr["id"]
        //                };
        //                pollutantData8Hr.Add(pollutant);
        //            }
        //            else
        //            {
        //                var pollutant = new PollutantData8Hr { 

        //                    Dev_id = (string)rdr["dev_id"],
        //                    Time = (DateTime)rdr["time"],
        //                    O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
        //                    //CO = (double?)rdr["co"],  //ugm3
        //                    Id = (int)rdr["id"],
        //                };

        //                pollutantData8Hr.Add(pollutant);
        //            }
        //        }
        //    connection.Close();
        //}

        //public void Pull1hrData(Sensor s)
        //{
        //    DateTime nowDay = DateTime.Now;
        //    string currentHour = nowDay.ToString("HH:MM");
        //    DateTime pastHrs = nowDay.AddHours(-1);
        //    string pastTime = pastHrs.ToString("HH:MM");

        //    //pulls closest sensor name
        //    string sensorLocation = s.Name;
        //    string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");
        //    SqlConnection connection = new SqlConnection(connectionstring);

        //    connection.Open();

        //    string sql;

        //    if (sensorLocation.Contains("graq"))
        //    {
        //        sql = $"EXEC OSTSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
        //    }
        //    else
        //    {
        //        sql = $"EXEC SimmsSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
        //    }

        //    SqlCommand com = new SqlCommand(sql, connection);
        //    SqlDataReader rdr = com.ExecuteReader();
        //    while (rdr.Read())
        //    {
        //        if (sensorLocation.Contains("graq"))
        //        {
        //            var pollutant = new PollutantData
        //            {
        //                Dev_id = (string)rdr["dev_id"],
        //                Time = (DateTime)rdr["time"],
        //                O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
        //                Id = (int)rdr["id"]
        //            };
        //            pollutantData1Hr.Add(pollutant);
        //        }
        //        else
        //        {
        //            var pollutant = new PollutantData
        //            {
        //                Dev_id = (string)rdr["dev_id"],
        //                Time = (DateTime)rdr["time"],
        //                O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
        //                NO2 = Math.Round((double)rdr["no2"], 0), //ugm3
        //                SO2 = Math.Round((double)rdr["so2"], 0), //ugm3
        //                Id = (int)rdr["id"]
        //            };

        //            pollutantData1Hr.Add(pollutant);
        //        }
        //    }
        //    connection.Close();
        //}

        //public void Pull24hrData(Sensor s)
        //{
        //    DateTime nowDay = DateTime.Now;
        //    string currentHour = nowDay.ToString("HH:MM");

        //    //pulls closest sensor name
        //    string sensorLocation = s.Name;
        //    string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");
        //    SqlConnection connection = new SqlConnection(connectionstring);

        //    connection.Open();

        //    string sql;

        //    if (sensorLocation.Contains("graq"))
        //    {
        //        sql = $"EXEC OSTSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-27 {currentHour}', @endtime = '2019-03-28 {currentHour}';";
        //    }
        //    else
        //    {
        //        sql = $"EXEC SimmsSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-27 {currentHour}', @endtime = '2019-03-28 {currentHour}';";
        //    }

        //    SqlCommand com = new SqlCommand(sql, connection);
        //    SqlDataReader rdr = com.ExecuteReader();
        //    while (rdr.Read())
        //    {
        //        if (sensorLocation.Contains("graq"))
        //        {
        //            var pollutant = new PollutantData24Hr
        //            {
        //                Dev_id = (string)rdr["dev_id"],
        //                Time = (DateTime)rdr["time"],
        //                Pm25 = Math.Round((double)rdr["pm25"], 1), //ugm3
        //                PM10 = Math.Round((double)rdr["pm10Average"], 1), //ugm3
        //                Id = (int)rdr["id"]
        //            };
        //            pollutantData24Hr.Add(pollutant);
        //        }
        //        else
        //        {
        //            var pollutant = new PollutantData24Hr
        //            {
        //                Dev_id = (string)rdr["dev_id"],
        //                Time = (DateTime)rdr["time"],
        //                Pm25 = Math.Round((double)rdr["pm25"], 1), //ugm3
        //                Id = (int)rdr["id"]
        //            };

        //            pollutantData24Hr.Add(pollutant);
        //        }
        //    }
        //    connection.Close();
        //}
        public IActionResult Index()
        {
            List<AQIs> AQIList = APIController.GetListAQI("48127");
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


        public static List<double> getFutureAQIs(double O3Reading)
        {
            List<WeatherDataFromAPI> weatherForecast = APIController.GetWeatherForcast();

            List<double> futureAQIs = new List<double>();

            for (int i = 0; i < 3; i++)
            {
                futureAQIs.Add(AQICalculations.WeatherForecastEquation(weatherForecast, i, O3Reading));
            }

            return futureAQIs;
        }
    }
}