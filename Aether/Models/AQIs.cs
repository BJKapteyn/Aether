using System;
using Newtonsoft.Json.Linq;

namespace Aether.Models
{
    public class AQIs
    {
        public string Pollutant { get; set; }
        public int AQI { get; set; }
        public string Rating { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Hour { get; set; }

        public AQIs()
        {
        }

        public AQIs(JToken jt, int index)
        {
            Pollutant = jt[index]["ParameterName"].ToString();
            AQI = (int)jt[index]["AQI"];
            Rating = jt[index]["Category"]["Name"].ToString();
            City = jt[index]["ReportingArea"].ToString();
            Date = jt[index]["DateObserved"].ToString();
            Hour = jt[index]["HourObserved"].ToString();
        }

    }
}

