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

                if(hours >= -1)
                {
                    try
                    {
                        if (sensorLocation.Contains("graq"))
                        { 
                            pollutant.O3 = AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48); //ppm
                        }
                        else
                        {
                            pollutant.O3 = AQICalculations.ConvertPPBtoPPM((double)rdr["o3"]); //ppb
                        }

                    } 
                    catch(InvalidCastException)
                    {
                        pollutant.O3 = 0;
                    }
                    try
                    {
                        pollutant.NO2 = (double)rdr["no2"]; //ppb
                    }//ugm3
                    catch (InvalidCastException)
                    {
                        pollutant.NO2 = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        pollutant.NO2 = 0;
                    }
                    try
                    {
                        pollutant.SO2 = (double)rdr["so2"]; //ppb
                    }
                    catch(InvalidCastException)
                    {
                        pollutant.SO2 = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        pollutant.SO2 = 0;
                    }
                }
                else if(hours >= -8)
                {
                    try
                    {
                        if (sensorLocation.Contains("graq"))
                        {
                            pollutant.O3 = AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48); //ppm
                        }
                        else
                        {
                            pollutant.O3 = AQICalculations.ConvertPPBtoPPM((double)rdr["o3"]); //ppb
                        }
                    }
                    catch(InvalidCastException)
                    {
                        pollutant.O3 = 0;
                    }
                    try
                    {
                        pollutant.CO = (double)rdr["co"]; //ppm
                    }
                    catch (InvalidCastException)
                    {
                        pollutant.CO = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        pollutant.CO = 0;
                    }
                }
                else
                {
                    try
                    {
                        pollutant.PM10 = (double)rdr["pm10"]; //ugm3
                    }
                    catch (InvalidCastException)
                    {
                        pollutant.PM10 = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        pollutant.PM10 = 0;
                    }
                    try
                    {
                        pollutant.PM25 = (double)rdr["pm25"]; //ugm3
                    }
                    catch(InvalidCastException)
                    {
                        pollutant.PM25 = 0;
                    }
                }

            }
            connection.Close();

            return pollutant;
        }

    }
}
