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
        public static List<PollutantData> pollutantData = new List<PollutantData>();
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
            string sensorLocation = "0004a30b0024358c";
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
                        PM10 = Math.Round((double)rdr["pm10"], 1), //ugm3
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

        //public static List<double> HistoricData(Sensor s)
        //{
        //    //0 index is numbers for one week, 1 is number for one month
        //    List<double> oneWeekValues = new List<double>();

        //    double sensorData;

        //    int Day = 21;
        //    string currentHour = "20"; //DateTime.Now.ToString("HH");
        //    for (int i = 0; i < 7; i++)
        //    {
        //        if (Day > 31)
        //        {
        //            Day = 1;
        //        }
        //        //grabs the day one month ago and then increments it
        //        string sDay = Day.ToString();
        //        if (sDay.Length == 1)
        //        {
        //            sDay = "0" + Day;
        //        }

        //        string monthAgoTime = $"2019-03-{sDay}T{currentHour}";
        //        sensorData = PullHistoricData(s, 20, monthAgoTime);
        //        if (sensorData != 0)
        //        {
        //            oneWeekValues.Add(Math.Round(sensorData));
        //        }
        //        Day++;
        //    }
        //    return oneWeekValues;
        //} 
        public static double CalculateAQI(double pollutantPPM, int breakpointIndex, int pollutantIndex)
        {
            // using 1h reading
            double airQuailtyIndex = CalculationController.CalculateAQI(pollutantPPM, breakpointIndex, pollutantIndex);

            return airQuailtyIndex;
        }

        //public static void PredictAQI()
        //{
        //    UGM3 = PollutantController.ConvertToUGM3(PollutantController.eighthourO3);
        //    for (int j = 0; j < 4; j++)
        //    {
        //        // using weather data to forecast tomorrow's AQI (index 1 = 24h)
        //        double futureWeatherForO3 = WeatherController.WeatherForecastEquation(weather, j, UGM3);
        //        // convert from UG/M3 to PPM 
        //        double futureAQIO3PPM = PollutantController.UGM3ConvertToPPM(futureWeatherForO3, 48);
        //        int EPABreakpointIndex = PollutantController.EPABreakpointTable(futureAQIO3PPM);
        //        double FutureAQIForO3 = PollutantController.CalculateO3AQI(futureAQIO3PPM, EPABreakpointIndex, indexAndOneorEight[0]);
        //        // add future AQIs to list
        //        ForecastedAQI.Add(FutureAQIForO3);
        //    }
        //}
    }
}