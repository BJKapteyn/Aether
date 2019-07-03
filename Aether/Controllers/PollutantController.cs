using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Aether.Controllers
{
    public class PollutantController : Controller
    {
        public static List<PollutantData> PollutantData = new List<PollutantData>();
        public static List<double> AQIData = new List<double>();

        private readonly IConfiguration configuration;

        public PollutantController(IConfiguration config)
        {
            this.configuration = config;
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
                sql = $"EXEC SelectReadings @dev_id = {sensorLocation}, @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
            }
            else
            {
                sql = $"EXEC SelectReadings @dev_id = {sensorLocation}, @time = '2019-03-28 {pastTime}', @endtime = '2019-03-28 {currentHour}';";
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
                            Id = (int)rdr["id"]
                        };
                        PollutantData.Add(pollutant);
                    }
                    else
                    {
                        var pollutant = new PollutantData
                        {
                            Dev_id = (string)rdr["dev_id"],
                            Time = (DateTime)rdr["time"],
                            O3 = Math.Round(CalculationController.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
                            Pm25 = Math.Round((double)rdr["pm25"], 1), //ugm3

                            Id = (int)rdr["id"]
                        };

                        PollutantData.Add(pollutant);
                    }
              }
            connection.Close();

            if (sensorLocation.Contains("graq"))
            {
                var ostO3Sum = PollutantData.Sum(x => x.O3);
                var ostO35Average = Math.Round(ostO3Sum / PollutantData.Count, 3);
                AQIData.Add(ostO35Average);

                var ostPM25Sum = PollutantData.Sum(x => x.Pm25);
                var ostPM25Average = Math.Round(ostPM25Sum / PollutantData.Count, 1);
                AQIData.Add(ostPM25Average);
            }
            else
            {

            }
        }
    }
}