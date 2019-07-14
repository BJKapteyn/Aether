using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Controllers;

namespace Aether.Models
{
    public class PollutantData
    {
        public int Id { get; set; }
        public string Dev_id { get; set; }
        public DateTime Time { get; set; }
        public double? O3 { get; set; }
        public int O3BPIndex { get; set; }

        public double? PM25 { get; set; }
        public int PM25BPIndex { get; set; }

        public double? PM10 { get; set; }
        public int PM10BPIndex { get; set; }

        public double? CO { get; set; }
        public int COBPIndex { get; set; }

        public double? NO2 { get; set; }
        public int NO2BPIndex { get; set; }

        public double? SO2 { get; set; }
        public int SO2BPIndex { get; set; }


        public PollutantData(List<PollutantData> PD)
        {
            O3 = AQICalculations.PollutantAverage(PD, x => x.O3);
            PM25 = AQICalculations.PollutantAverage(PD, x => x.PM25);
            PM10 = AQICalculations.PollutantAverage(PD, x => x.PM10);
            CO = AQICalculations.PollutantAverage(PD, x => x.CO);
            NO2 = AQICalculations.PollutantAverage(PD, x => x.NO2);
            SO2 = AQICalculations.PollutantAverage(PD, x => x.SO2);
        }
    }
}
