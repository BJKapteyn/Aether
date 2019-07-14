using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Controllers;

namespace Aether.Models
{
    public class Pollutants
    {
        public double? O3Average { get; set; }
        public int O3BPIndex { get; set; }

        public double? PM25Average { get; set; }
        public int PM25BPIndex { get; set; }

        public double? PM10Average { get; set; }
        public int PM10BPIndex { get; set; }

        public double? COAverage { get; set; }
        public int COBPIndex { get; set; }

        public double? NO2Average { get; set; }
        public int NO2BPIndex { get; set; }

        public double? SO2Average { get; set; }
        public int SO2BPIndex { get; set; }

        public Pollutants(List<PollutantData> PD)
        {
            O3Average = AQICalculations.PollutantAverage(PD, x => x.O3);
            PM25Average = AQICalculations.PollutantAverage(PD, x => x.PM25);
            PM10Average = AQICalculations.PollutantAverage(PD, x => x.PM10);
            COAverage = AQICalculations.PollutantAverage(PD, x => x.CO);
            NO2Average = AQICalculations.PollutantAverage(PD, x => x.NO2);
            SO2Average = AQICalculations.PollutantAverage(PD, x => x.SO2);
        }

        //public static List<Pollutants> GetPollutantTypes()
        //{
        //    List<Pollutants> pollutants = new List<Pollutants>();

        //    pollutants.Add(new Pollutant(
        //        "O3/8",
        //        new double[] { 0, 0.06, 0.076, 0.096, 0.116, 0.00001, 0.00001 },
        //        new double[] { 0.059, 0.075, 0.095, 0.115, 0.374, 0.00001, 0.00001 }));
        //    //0.00001 are null values
        //    pollutants.Add(new Pollutant(
        //        //pollutant/hr range
        //        "O3/1",
        //        //low values break point of O3 value from EPA
        //        new double[] { 0.00001, 0.00001, 0.125, 0.165, 0.205, 0.405, 0.505 },
        //        //high values break point value from EPA
        //        new double[] { 0.00001, 0.00001, 0.164, 0.204, 0.404, 0.504, 0.604 }));

        //    pollutants.Add(new Pollutant(
        //        "PM10/24",
        //        new double[] { 0, 55, 155, 255, 355, 425, 505 },
        //        new double[] { 54, 154, 254, 354, 424, 504, 604 }));

        //    pollutants.Add(new Pollutant(
        //        "PM2.5/24",
        //        new double[] { 0, 15.5, 40.5, 65.5, 150.5, 250.5, 350.5 },
        //        new double[] { 15.4, 40.4, 65.4, 150.4, 250.4, 350.4, 500.4 }));

        //    pollutants.Add(new Pollutant(
        //        "CO/8",
        //        new double[] { 0, 4.5, 9.5, 12.5, 15.5, 30.5, 40.5 },
        //        new double[] { 4.4, 9.4, 12.4, 15.4, 30.4, 40.4, 50.4 }));

        //    pollutants.Add(new Pollutant(
        //        "SO2/1",
        //        new double[] { 0, 36, 76, 186, 305, 605, 805 },
        //        new double[] { 35, 75, 185, 304, 604, 804, 1004 }));

        //    pollutants.Add(new Pollutant(
        //        "NO2/1",
        //        new double[] { 0, 54, 101, 361, 650, 1250, 1650 },
        //        new double[] { 53, 100, 360, 649, 1249, 1649, 2049 }));

        //    pollutants.Add(new Pollutant(
        //        "AQI",
        //        new double[] { 0, 51, 101, 151, 201, 301, 401 },
        //        new double[] { 50, 100, 150, 200, 300, 400, 500 }));

        //    return pollutants;
        //}
    }
}
