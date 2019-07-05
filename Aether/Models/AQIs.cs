using System;
using Newtonsoft.Json.Linq;

namespace Aether.Models
{
    public class AQIs
    {
        public string Pollutant { get; set; }
        public int AQI { get; set; }
        public string City { get; set; }

        public AQIs()
        {
        }

        public AQIs(JToken jt, int index)
        {
            Pollutant = jt[index]["ParameterName"].ToString();
            AQI = (int)jt[index]["AQI"];
            City = jt[index]["ReportingArea"].ToString();
        }

    }
}

