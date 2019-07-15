using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Models
{
    public class ReferenceCodeThrowAway
    {
        //1hr
        //                Dev_id = (string)rdr["dev_id"],
        //                Time = (DateTime)rdr["time"],
        //                O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3), //ppm
        //                NO2 = Math.Round((double)rdr["no2"], 0), //ugm3
        //                SO2 = Math.Round((double)rdr["so2"], 0), //ugm3
        //                Id = (int)rdr["id"]
        //8hr
        //                    Dev_id = (string)rdr["dev_id"],
        //                    Time = (DateTime)rdr["time"],
        //                    //CO = (double?)rdr["co"],  //ugm3
        //                    Id = (int)rdr["id"],
        //24hr
        //                Dev_id = (string)rdr["dev_id"],
        //                Time = (DateTime)rdr["time"],
        //                Id = (int)rdr["id"]
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
    }
}
