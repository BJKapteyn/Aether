using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Aether.Controllers;

namespace Aether.Models
{
    public class PullOSTData
    {
        public List<PollutantData> Data { get; set; }

        public PullOSTData()
        {
        }

        public PullOSTData(Sensor s, int hours, IConfiguration c)
        {
            Data = PullData(s, hours, c);
        }

        public List<PollutantData> PullData(Sensor s, int hours, IConfiguration c)
        {
            List<PollutantData> ostData = new List<PollutantData>();
            //will add hours soon
            DateTime nowDay = DateTime.Now;
            string lasthour = nowDay.AddHours(hours).ToString("HH:mm");
            string currentHour = nowDay.ToString("HH:mm");
            string sql;
            //pulls closest sensor name
            string dbSensorCall = "AVG" + s.Name;

            string connectionstring = c.GetConnectionString("DefaultConnectionstring");

            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();

            //will change when implementing variable days/current time
            if(hours <= -24)
            {
                sql = $"EXEC {dbSensorCall} @time = '2019-07-11T{currentHour}', @endtime = '2019-07-12T{currentHour}';";
            }
            else
            {

                sql = $"EXEC {dbSensorCall} @time = '2019-07-12T{lasthour}', @endtime = '2019-07-12T{currentHour}';";
            }

            SqlCommand com = new SqlCommand(sql, connection);
            SqlDataReader rdr = com.ExecuteReader();

            while (rdr.Read())
            {
                PollutantData pollutant = new PollutantData();
                try
                {
                    pollutant.O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3);
                }
                catch(InvalidCastException)
                {
                    pollutant.O3 = 0;
                }

                if (hours <= -24)
                {
                    try
                    {
                        pollutant.PM25 = Math.Round((double)rdr["pm25"], 1);
                    }//ugm3
                    catch(InvalidCastException)
                    {
                        pollutant.PM25 = 0;
                    }
                    try
                    {
                        pollutant.PM10 = Math.Round((double)rdr["pm10"], 1);
                    }//ugm3
                    catch(InvalidCastException)
                    {
                        pollutant.PM10 = 0;
                    }
                }

                ostData.Add(pollutant);
            }

            connection.Close();
            return ostData;

        }


        //public List<PollutantData> OSTData(Sensor s, int hours, IConfiguration c)
        //{
        //    SqlDataReader rdr = PullData(s, hours, c);
        //    List<PollutantData> ostData = new List<PollutantData>();

        //    while (rdr.Read())
        //    {
        //        PollutantData pollutant = new PollutantData();

        //        pollutant.Dev_id = (string)rdr["dev_id"];
        //        pollutant.Time = (DateTime)rdr["time"];
        //        pollutant.Id = (int)rdr["id"];
        //        if(hours >= 24)
        //        {
        //            pollutant.Pm25 = Math.Round((double)rdr["pm25"], 1); //ugm3
        //            pollutant.PM10 = Math.Round((double)rdr["pm10Average"], 1); //ugm3
        //        }

        //        ostData.Add(pollutant);
        //    }

        //    return ostData;
        //}
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
