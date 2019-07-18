using System;
using Newtonsoft.Json.Linq;

namespace Aether.Models
{
    public class AQIs
    {
        public string Pollutant { get; set; }
        public int O3AQI { get; set; }
        public int PM25AQI { get; set; }
        public string Rating { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Hour { get; set; }

        public AQIs()
        {
        }

        public AQIs(JToken jt)
        {
            Pollutant = jt[0]["ParameterName"].ToString();
            O3AQI = (int)jt[0]["AQI"];
            PM25AQI = (int)jt[1]["AQI"];
            Rating = jt[0]["Category"]["Name"].ToString();
            City = jt[0]["ReportingArea"].ToString();
            Date = jt[0]["DateObserved"].ToString();
            Hour = jt[0]["HourObserved"].ToString();
        }

    }
}

