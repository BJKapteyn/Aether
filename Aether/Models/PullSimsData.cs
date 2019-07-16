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

                if(hours <= -1)
                {
                    try
                    {
                        if (sensorLocation.Contains("graq"))
                        { 
                            pollutant.O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3); //ppm
                        }
                        else
                        {
                            pollutant.O3 = Math.Round(AQICalculations.ConvertPPBtoPPM((double)rdr["o3"]), 3); //ppb
                        }

                    } 
                    catch(InvalidCastException)
                    {
                        pollutant.O3 = 0;
                    }
                    try
                    {
                        pollutant.NO2 = Math.Round((double)rdr["no2"], 0); //ppb
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
                        pollutant.SO2 = Math.Round((double)rdr["so2"], 0); //ppb
                    }
                    catch(InvalidCastException)
                    {
                        pollutant.SO2 = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        pollutant.NO2 = 0;
                    }
                }
                else if(hours <= -8)
                {
                    try
                    {
                        if (sensorLocation.Contains("graq"))
                        {
                            pollutant.O3 = Math.Round(AQICalculations.UGM3ConvertToPPM((double)rdr["o3"], 48), 3); //ppm
                        }
                        else
                        {
                            pollutant.O3 = Math.Round(AQICalculations.ConvertPPBtoPPM((double)rdr["o3"]), 3); //ppb
                        }
                    }
                    catch(InvalidCastException)
                    {
                        pollutant.O3 = 0;
                    }
                    try
                    {
                        pollutant.CO = Math.Round((double)rdr["co"], 1); //ppm
                    }
                    catch (InvalidCastException)
                    {
                        pollutant.CO = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        pollutant.NO2 = 0;
                    }
                }
                else
                {
                    try
                    {
                        pollutant.PM10 = Math.Round((double)rdr["pm10"], 0); //ugm3
                    }
                    catch (InvalidCastException)
                    {
                        pollutant.PM10 = 0;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        pollutant.NO2 = 0;
                    }
                    try
                    {
                        pollutant.PM25 = Math.Round((double)rdr["pm25"], 1); //ugm3
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
