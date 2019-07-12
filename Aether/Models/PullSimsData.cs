using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Aether.Controllers;

namespace Aether.Models
{
    public class PullSimsData
    {
        public PullSimsData()
        {

        }

        public List<PollutantData> PullData(Sensor s, int hours, IConfiguration configuration)
        {
            List<PollutantData> pollutantData = new List<PollutantData>();

            DateTime nowDay = DateTime.Now;
            string currentHour = nowDay.ToString("HH:MM");
            string lastHour = nowDay.AddHours(hours).ToString("HH:mm");

            //pulls closest sensor name
            string sensorLocation = s.Name;
            string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();

            string sql;

            if (hours >= 24)
            {
                sql = $"EXEC SimmsSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {currentHour}', @endtime = '2019-03-28 {currentHour}';";
            }
            else
            {
                sql = $"EXEC SimmsSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {lastHour}', @endtime = '2019-03-28 {currentHour}';";
            }

            SqlCommand com = new SqlCommand(sql, connection);
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var pollutant = new PollutantData();

                pollutant.Dev_id = (string)rdr["dev_id"];
                pollutant.Time = (DateTime)rdr["time"];
                pollutant.Id = (int)rdr["id"];

                if(hours >= 1)
                {
                    pollutant.O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3); //ppm
                    pollutant.NO2 = Math.Round((double)rdr["no2"], 0); //ugm3
                    pollutant.SO2 = Math.Round((double)rdr["so2"], 0); //ugm3
                }
                else if(hours >= 8)
                {
                    pollutant.O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3); //ppm
                }
                else
                {
                    pollutant.Pm25 = Math.Round((double)rdr["pm25"], 1); //ugm3
                }

                pollutantData.Add(pollutant);
            }
            connection.Close();

            return pollutantData;
        }

    }
}
