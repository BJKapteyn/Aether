using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Models
{
    public class DisplayToUserInformation
    {
        public string PollutantWarning { get; set; }
        public double AQIO3 { get; set; }
        public double O3Avg { get; set; }
        public double AQIPM10 { get; set; }
        public double AQIPM25 { get; set; }
        public double AQICO { get; set; }
        public double COAvg { get; set; }
        public double AQISO2 { get; set; }
        public double AQINO2 { get; set; }
        public double NO2Avg { get; set; }
        public double AQIPredicted1Day { get; set; }
        public double AQIPredicted3Day { get; set; }
        public double AQIPredicted5Day { get; set; }
        public double AQIEPA { get; set; }
        public string SensorName { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
        public string Address { get; set; }
        public string Recommendations { get; set; }
        //public Sensor UserSensor { get; set; }
        public string AQIColor1 { get; set; }
        public string AQIColor2 { get; set; }
        public string AQIColor3 { get; set; }
        public double Distance { get; set; }
        public string Suggestion { get; set; }
        public double HighestAQI { get; set; }
        public Sensor Sensor { get; set; }
        //public List<WeatherDataFromAPI> Weather { get; set; }
        //public List<Sensor> TwoClosestSensors { get; set; }

        public List<double> HistoricData { get; set; }

        public double Factorylat { get; set; }
        public double Factorylong { get; set; }

        public List<FutureAQIs> FutureAQIs { get; set; } // index 0 = 1 day, index 1 = 3 day, index 2 = 5 day
        //                                                  .O3AQI, .COAQI, .NO2AQI
        public void CalculateHighestAQI()
        {
            double highestAQI = 0;
            List<double> aqis = new List<double>();
            aqis.Add(AQIO3);
            aqis.Add(AQIPM25);
            aqis.Add(AQIPM10);
            aqis.Add(AQISO2);
            aqis.Add(AQINO2);
            aqis.Add(AQICO);

            foreach(double d in aqis)
            {
                if(d > highestAQI)
                {
                    highestAQI = d;
                }
            }

            HighestAQI = highestAQI;
        }

        public void AddColor()
        {
            double d = this.HighestAQI;
            string[] hexColors = { "#00e400", "#ffff00", "#ff7e00", "#ff0000", "#8f3f97", "#7e0023" };
            //                      Green     Yellow    Orange    Red       Purple    Maroon

            if(d >= 0 && d <=50)
            {
                AQIColor1 = hexColors[0];
            }
            else if(d >= 51 && d <= 100)
            {
                AQIColor1 = hexColors[1];
            }
            else if(d >=101 && d <=150)
            {
                AQIColor1 = hexColors[2];
            }
            else if(d >= 151 && d <= 200)
            {
                AQIColor1 = hexColors[3];
            }
            else if(d >= 201 && d <= 300)
            {
                AQIColor1 = hexColors[4];
            }
            else
            {
                AQIColor1 = hexColors[5];
            }
        }
    }
}
