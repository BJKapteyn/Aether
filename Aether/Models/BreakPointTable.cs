﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Models
{
    public class BreakPointTable
    {
        public string Name { get; set; }
        public double[] Low { get; set; }
        public double[] High { get; set; }

        public BreakPointTable(string name, double[] low, double[] high)
        {
            Name = name;
            Low = low;
            High = high;
        }

        public static BreakPointTable O38hr = new BreakPointTable(
            "O3/8",
            new double[] { 0, 0.06, 0.076, 0.096, 0.116, 0.00001, 0.00001 },
            new double[] { 0.059, 0.075, 0.095, 0.115, 0.374, 0.00001, 0.00001 });

        public static BreakPointTable O31hr = new BreakPointTable(
            "O3/1",
            new double[] { 0.00001, 0.00001, 0.125, 0.165, 0.205, 0.405, 0.505 },
            new double[] { 0.00001, 0.00001, 0.164, 0.204, 0.404, 0.504, 0.604 });

        public static BreakPointTable PM1024hr = new BreakPointTable(
            "PM10/24",
            new double[] { 0, 55, 155, 255, 355, 425, 505 },
            new double[] { 54, 154, 254, 354, 424, 504, 604 });

        public static BreakPointTable PM2524hr = new BreakPointTable(
            "PM2.5/24",
            new double[] { 0, 15.5, 40.5, 65.5, 150.5, 250.5, 350.5 },
            new double[] { 15.4, 40.4, 65.4, 150.4, 250.4, 350.4, 500.4 });

        public static BreakPointTable CO8hr = new BreakPointTable(
            "CO/8",
            new double[] { 0, 4.5, 9.5, 12.5, 15.5, 30.5, 40.5 },
            new double[] { 4.4, 9.4, 12.4, 15.4, 30.4, 40.4, 50.4 });

        public static BreakPointTable SO21hr = new BreakPointTable(
            "SO2 / 1",
            new double[] { 0, 36, 76, 186, 305, 605, 805 },
            new double[] { 35, 75, 185, 304, 604, 804, 1004 });

        public static BreakPointTable NO21hr = new BreakPointTable(
            "NO2/1",
            new double[] { 0, 54, 101, 361, 650, 1250, 1650 },
            new double[] { 53, 100, 360, 649, 1249, 1649, 2049 });

        public static BreakPointTable AQI = new BreakPointTable(
            "AQI",
            new double[] { 0, 51, 101, 151, 201, 301, 401 },
            new double[] { 50, 100, 150, 200, 300, 400, 500 });



        public static List<BreakPointTable> GetPollutantTypes()
        {
            List<BreakPointTable> pollutants = new List<BreakPointTable>
            {
                new BreakPointTable(
                "O3/8",
                new double[] { 0, 0.06, 0.076, 0.096, 0.116, 0.00001, 0.00001 },
                new double[] { 0.059, 0.075, 0.095, 0.115, 0.374, 0.00001, 0.00001 }),
                //0.00001 are null values
                new BreakPointTable(
                //pollutant/hr range
                "O3/1",
                //low values break point of O3 value from EPA
                new double[] { 0.00001, 0.00001, 0.125, 0.165, 0.205, 0.405, 0.505 },
                //high values break point value from EPA
                new double[] { 0.00001, 0.00001, 0.164, 0.204, 0.404, 0.504, 0.604 }),

                new BreakPointTable(
                "PM10/24",
                new double[] { 0, 55, 155, 255, 355, 425, 505 },
                new double[] { 54, 154, 254, 354, 424, 504, 604 }),

                new BreakPointTable(
                "PM2.5/24",
                new double[] { 0, 15.5, 40.5, 65.5, 150.5, 250.5, 350.5 },
                new double[] { 15.4, 40.4, 65.4, 150.4, 250.4, 350.4, 500.4 }),

                new BreakPointTable(
                "CO/8",
                new double[] { 0, 4.5, 9.5, 12.5, 15.5, 30.5, 40.5 },
                new double[] { 4.4, 9.4, 12.4, 15.4, 30.4, 40.4, 50.4 }),

                new BreakPointTable(
                "SO2/1",
                new double[] { 0, 36, 76, 186, 305, 605, 805 },
                new double[] { 35, 75, 185, 304, 604, 804, 1004 }),

                new BreakPointTable(
                "NO2/1",
                new double[] { 0, 54, 101, 361, 650, 1250, 1650 },
                new double[] { 53, 100, 360, 649, 1249, 1649, 2049 }),

                new BreakPointTable(
                "AQI",
                new double[] { 0, 51, 101, 151, 201, 301, 401 },
                new double[] { 50, 100, 150, 200, 300, 400, 500 })
            };

            return pollutants;
        }
    }
}
