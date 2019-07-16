using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Aether.Models;
using Aether.Controllers;

namespace Aether.Models
{
    public class PullSimsData
    {

        public double? O3Average { get; set; }
        public int O3BPIndex { get; set; }
        public double O3AQI { get; set; }

        public double? PM25Average { get; set; }
        public int PM25BPIndex { get; set; }
        public double PM25AQI { get; set; } = 0;

        public double? PM10Average { get; set; }
        public int PM10BPIndex { get; set; }
        public double PM10AQI { get; set; } = 0;

        public double? COAverage { get; set; }
        public int COBPIndex { get; set; }
        public double COAQI { get; set; } = 0;

        public double? NO2Average { get; set; }
        public int NO2BPIndex { get; set; }
        public double NO2AQI { get; set; } = 0;

        public double? SO2Average { get; set; }
        public int SO2BPIndex { get; set; }
        public double SO2AQI { get; set; } = 0;

        public PollutantData Data {get; set;}

        public PullSimsData()
        {

        }

        public PullSimsData(Sensor s, int hours, IConfiguration c)
        {
            Data = PullData(s, hours, c);
        }

        public PollutantData PullData(Sensor s, int hours, IConfiguration configuration)
        {
            PollutantData pollutant = new PollutantData();

            DateTime nowDay = DateTime.Now;
            string currentHour = nowDay.ToString("HH:mm");
            string lastHour = nowDay.AddHours(hours).ToString("HH:mm");
            string dbSensorCall = "AVG" + s.Name;
            //pulls closest sensor name
            string sensorLocation = s.Name;
            string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");
            SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();

            string sql;

            if (hours <= -24)
            {
                sql = $"EXEC {dbSensorCall} @time = '2019-07-11T{currentHour}', @endtime = '2019-07-12T{currentHour}';";
            }
            else
            {
                sql = $"EXEC {dbSensorCall} @time = '2019-07-12T{lastHour}', @endtime = '2019-07-12T{currentHour}';";
            }

            SqlCommand com = new SqlCommand(sql, connection);
            SqlDataReader rdr = com.ExecuteReader();

            //adds the pollutants based on the hour given as a param
            while (rdr.Read())
            {

                if(hours <= -1)
                {
                    try
                    {
                        if (sensorLocation.Contains("graq"))
                        {
                            O3Average = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3); //ppm
                        }
                        else
                        {
                            O3Average = Math.Round(AQICalculations.ConvertPPBtoPPM((double)rdr["o3"]), 3); //ppb
                        }
                    } 
                    catch(InvalidCastException)
                    {
                        O3Average = 0;
                    }
                    try
                    {
                        NO2Average = Math.Round((double)rdr["no2"], 0); //ppb
                    }//ugm3
                    catch (InvalidCastException)
                    {
                        NO2Average = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        NO2Average = 0;
                    }
                    try
                    {
                        SO2Average = Math.Round((double)rdr["so2"], 0); //ppb
                    }
                    catch(InvalidCastException)
                    {
                        SO2Average = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        SO2Average = 0;
                    }
                }
                else if(hours <= -8)
                {
                    try
                    {
                        if (sensorLocation.Contains("graq"))
                        {
                            O3Average = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3); //ppm
                        }
                        else
                        {
                            O3Average = Math.Round(AQICalculations.ConvertPPBtoPPM((double)rdr["o3"]), 3); //ppb
                        }
                    }
                    catch(InvalidCastException)
                    {
                        O3Average = 0;
                    }
                    try
                    {
                        COAverage = Math.Round((double)rdr["co"], 1); //ppm
                    }
                    catch (InvalidCastException)
                    {
                        COAverage = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                       COAverage = 0;
                    }
                }
                else
                {
                    try
                    {
                        PM10Average = Math.Round((double)rdr["pm10"], 0); //ugm3
                    }
                    catch (InvalidCastException)
                    {
                        PM10Average = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        PM10Average = 0;
                    }
                    try
                    {
                        PM25Average = Math.Round((double)rdr["pm25"], 1); //ugm3
                    }
                    catch(InvalidCastException)
                    {
                        PM25Average = 0;
                    }
                }

            }
            connection.Close();

            return pollutant;
        }

    }
}
