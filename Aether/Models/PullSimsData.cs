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
                sql = $"EXEC {dbSensorCall} @time = '2019-07-16T{currentHour}', @endtime = '2019-07-17T{currentHour}';";
            }
            else
            {
                sql = $"EXEC {dbSensorCall} @time = '2019-07-17T{lastHour}', @endtime = '2019-07-17T{currentHour}';";
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
                else if(hours >= -8)
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
        //left off here--------------------------------------------------------------------------
        public void AddAQIs()
        {
            if (this.O3Average > 0 && !(this.O3AQI > 0))
            {
                //if the reading is bad, the breakpoint index int will be high and out of range of the table
                try
                {
                    this.O3BPIndex = AQICalculations.BreakpointIndexCalculation((double)O3Average, BreakPointTable.O38hr);
                    this.O3AQI = AQICalculations.AQIEquation((double)O3Average, O3BPIndex, BreakPointTable.O38hr);
                }
                catch (IndexOutOfRangeException)
                {
                    this.O3AQI = 0;
                }
            }

            if (this.PM25Average > 0 && !(this.PM25AQI > 0))
            {
                try
                {
                    this.PM25BPIndex = AQICalculations.BreakpointIndexCalculation((double)PM25Average, BreakPointTable.PM2524hr);
                    this.PM25AQI = AQICalculations.AQIEquation((double)PM25Average, PM25BPIndex, BreakPointTable.PM2524hr);
                }
                catch (IndexOutOfRangeException)
                {
                    this.PM25AQI = 0;
                }

            }

            if (this.PM10Average > 0 && !(this.PM10AQI > 0) )
            {
                try
                {
                    this.PM10BPIndex = AQICalculations.BreakpointIndexCalculation((double)PM10Average, BreakPointTable.PM1024hr);
                    this.PM10AQI = AQICalculations.AQIEquation((double)PM10Average, PM10BPIndex, BreakPointTable.PM1024hr);
                }
                catch (IndexOutOfRangeException)
                {
                    this.PM10AQI = 0;
                }
            }

            if (this.COAverage > 0 && !(this.COAQI > 0))
            {
                try
                {
                    this.COBPIndex = AQICalculations.BreakpointIndexCalculation((double)COAverage, BreakPointTable.CO8hr);
                    this.COAQI = AQICalculations.AQIEquation((double)COAverage, COBPIndex, BreakPointTable.CO8hr);
                }
                catch (IndexOutOfRangeException)
                {
                    this.COAQI = 0;
                }
            }
            if (this.NO2Average > 0 && !(this.NO2AQI > 0))
            {
                try
                {
                    this.NO2BPIndex = AQICalculations.BreakpointIndexCalculation((double)NO2Average, BreakPointTable.NO21hr);
                    this.NO2AQI = AQICalculations.AQIEquation((double)NO2Average, NO2BPIndex, BreakPointTable.NO21hr);
                }
                catch (IndexOutOfRangeException)
                {
                    this.NO2AQI = 0;
                }
            }
            if (this.SO2Average > 0 && !(this.SO2AQI > 0))
            {
                try
                {
                    this.SO2BPIndex = AQICalculations.BreakpointIndexCalculation((double)SO2Average, BreakPointTable.SO21hr);
                    this.SO2AQI = AQICalculations.AQIEquation((double)SO2Average, SO2BPIndex, BreakPointTable.SO21hr);
                }
                catch (IndexOutOfRangeException)
                {
                    this.SO2AQI = 0;
                }
            }

        }

    }
}
