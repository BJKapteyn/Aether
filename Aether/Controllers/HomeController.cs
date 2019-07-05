using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aether.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Aether.Controllers
{
    public class HomeController : Controller
    {
        public static List<PollutantData> pollutantData = new List<PollutantData>();
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }

        public IActionResult AirQuality()
        {
            PullData();
            CalculationController.SumAndAveragePollutantReadings();

            CalculationController.BreakPointIndex();

            CalculationController.AQI();

            DisplayToUserInformation rv = new DisplayToUserInformation
            {
                AQIFirstO3 = CalculationController.pollutantAQIs[0],
                AQIPM10 = CalculationController.pollutantAQIs[1],
                AQIPM25 = CalculationController.pollutantAQIs[2],
                AQICO = CalculationController.pollutantAQIs[3],
                AQISO2 = CalculationController.pollutantAQIs[4],
                AQINO2 = CalculationController.pollutantAQIs[5]

                //AQISecondO3
                //AQIThirdO3
                //AQIPredictedTomorrow
                //AQIPredicted3Day
                //AQIPredicted5Day 
            };

            return View(rv);
        }
            //sensor s and number of hours past 
        public void PullData()
        {
                DateTime nowDay = DateTime.Now;
                string currentHour = nowDay.ToString("HH:MM");
                DateTime pastHrs = nowDay.AddHours(-1);
                string pastTime = pastHrs.ToString("HH:MM");

                //pulls closest sensor name
                string sensorLocation = "graqm0107";
                string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");
                SqlConnection connection = new SqlConnection(connectionstring);

                connection.Open();

                string sql;

                if (sensorLocation.Contains("graq"))
                {
                    sql = $"EXEC OSTSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
                }
                else
                {
                    sql = $"EXEC SimmsSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
                }

                SqlCommand com = new SqlCommand(sql, connection);
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    if (sensorLocation.Contains("graq"))
                    {
                        var pollutant = new PollutantData
                        {
                            Dev_id = (string)rdr["dev_id"],
                            Time = (DateTime)rdr["time"],
                            O3 = Math.Round(CalculationController.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
                            Pm25 = Math.Round((double)rdr["pm25"], 1), //ugm3
                            PM10 = Math.Round((double)rdr["pm10Average"], 1), //ugm3
                            Id = (int)rdr["id"]
                        };
                        pollutantData.Add(pollutant);
                    }
                    else
                    {
                        var pollutant = new PollutantData
                        {
                            Dev_id = (string)rdr["dev_id"],
                            Time = (DateTime)rdr["time"],
                            O3 = Math.Round(CalculationController.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
                            Pm25 = Math.Round((double)rdr["pm25"], 1), //ugm3
                            CO = Math.Round((double)rdr["co"], 1), //ugm3
                            NO2 = Math.Round((double)rdr["no2"], 0), //ugm3
                            SO2 = Math.Round((double)rdr["so2"], 0), //ugm3
                            Id = (int)rdr["id"]
                        };

                        pollutantData.Add(pollutant);
                    }
                }
            connection.Close();
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
