using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Models
{
    public class DisplayToUserInformation
    {
        public string PollutantWarning { get; set; }
        public double AQIToday { get; set; }
        public double AQIO3 { get; set; }
        public double AQIPM10 { get; set; }
        public double AQIPM25 { get; set; }
        public double AQICO { get; set; }
        public double AQISO2 { get; set; }
        public double AQINO2 { get; set; }
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
        //public List<WeatherDataFromAPI> Weather { get; set; }
        //public List<Sensor> TwoClosestSensors { get; set; }

        public List<double> HistoricData { get; set; }

        public double Factorylat { get; set; }
        public double Factorylong { get; set; }

        public List<FutureAQIs> FutureAQIs { get; set; } // index 0 = 1 day, index 1 = 3 day, index 2 = 5 day
        //                                                  O3AQI, COAQI, NO2AQI
    }
}
