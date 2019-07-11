﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Aether.Models
{
    public class PullData
    {
        private readonly IConfiguration configuration;

        public PullData(IConfiguration config)
        {
            configuration = config;
        }

        public SqlDataReader PullOSTData(Sensor s, int hours)
        {
            //will add hours soon
            DateTime nowDay = DateTime.Now;
            string currentHour = nowDay.ToString("HH:mm");
            string sql;
            //pulls closest sensor name
            string sensorLocation = s.Name;
            string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");

            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();

            //will change when implementing variable days/current time
            if(hours >= 24)
            {
                 sql = $"EXEC OSTSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-27 {currentHour}', @endtime = '2019-03-28 {currentHour}';";
            }
            else
            {
                sql = $"EXEC OSTSelectReadings @dev_id = '{sensorLocation}', @time = '2019-03-28 {currentHour}', @endtime = '2019-03-28 {currentHour}';";
            }

            SqlCommand com = new SqlCommand(sql, connection);
            SqlDataReader rdr = com.ExecuteReader();

            connection.Close();

            return rdr;
        }

        public List<PollutantData> OSTData(Sensor s, int hours)
        {
            SqlDataReader rdr = PullOSTData(s, hours);
            List<PollutantData> ostData = new List<PollutantData>();

            while (rdr.Read())
            {
                PollutantData pollutant = new PollutantData();

                pollutant.Dev_id = (string)rdr["dev_id"];
                pollutant.Time = (DateTime)rdr["time"];
                pollutant.Id = (int)rdr["id"];
                if(hours >= 24)
                {
                    pollutant.Pm25 = Math.Round((double)rdr["pm25"], 1); //ugm3
                    pollutant.PM10 = Math.Round((double)rdr["pm10Average"], 1); //ugm3
                }

                ostData.Add(pollutant);
            }

            return ostData;
        }
    }
//var pollutant = new PollutantData24Hr
//{
//    Dev_id = (string)rdr["dev_id"],
//    Time = (DateTime)rdr["time"],
//    Pm25 = Math.Round((double)rdr["pm25"], 1), //ugm3
//    Id = (int)rdr["id"]
//};

//pollutantData24Hr.Add(pollutant);
}