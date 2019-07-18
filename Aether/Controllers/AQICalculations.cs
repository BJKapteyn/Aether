using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet;

namespace Aether.Controllers
{
    public class AQICalculations : Controller
    {
        public static List<BreakPointTable> breakPointTable = BreakPointTable.GetPollutantTypes();

        //enter lambda expression under pollutantLamda to designate the pollutant you want averaged ex.x => x.O3
        //public static double PollutantAverage(List<PollutantData> pollutants, Func<PollutantData, IComparable> pollutantLamda)
        //{
        //    double pollutantAverage;
        //    double pollutantSum;
        //    List<PollutantData> PD = new List<PollutantData>(pollutants);
        //    PD.RemoveAll(x => ((double)pollutantLamda(x)) == 0);

        //    if (PD.Count > 0)
        //    {
        //        pollutantSum = PD.Sum(x => (double)pollutantLamda(x));
        //        pollutantAverage = Math.Round((pollutantSum / PD.Count), 3);
        //        return pollutantAverage;
        //    }

        //    return 0;
        //}

        public static double ConvertPPBtoPPM(double PollutantPPB)
        {
            //1000 ppm = 1000 ppb
            double PollutantPPM = PollutantPPB / 1000;
            return PollutantPPM;
        }

        public static double UGM3ConvertToPPM(double UGM3, double moleWeight)
        {
            double PPM = (UGM3 / (0.0409 * moleWeight)) / 1000;
            return PPM;
        }


        //Calculate what breakpoint index to use
        public static int BreakpointIndexCalculation(double Pollutant, BreakPointTable B)
        {
            int breakPointIndex = 0;
            if (Pollutant != 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    double low = B.Low[i];
                    double high = B.High[i];

                    if (Pollutant >= low && Pollutant <= high)
                    {
                        breakPointIndex = i;
                        break;
                    }
                }
            }
            else
            {
                breakPointIndex = int.MaxValue;
            }

            return breakPointIndex;
        }

        public static double AQIEquation(double pollutantReading, int breakpointIndex, BreakPointTable breakPoint)
        {
            //index 7 in breakpoint table is the AQI
            double Ihi = breakPointTable[7].High[breakpointIndex];
            double Ilo = breakPointTable[7].Low[breakpointIndex];
            double BPhi = breakPoint.High[breakpointIndex];
            double BPlow = breakPoint.Low[breakpointIndex];
            double Cp = pollutantReading;

            double AQIForPollutant = ((Ihi - Ilo) / (BPhi - BPlow)) * (Cp - BPlow) + Ilo;
            return AQIForPollutant;
        }

        // overload in case you only want to bring in O3 data -- NOT IMPLEMENTED IN CONTROLLER YET
        public static FutureAQIs AQIForecastEquation(List<WeatherDataFromAPI> weatherTime, int index, double eightHourO3)
        {

            double FutureO3AQI = (double)(5.3 * weatherTime[index].WindSpeed) + (double)(0.4 * weatherTime[index].TemperatureC) +
                (double)(0.1 * weatherTime[index].Humidity) + ((double)0.7 * eightHourO3);

            FutureAQIs futureAQIO3Only = new FutureAQIs((int)Math.Round(FutureO3AQI), 0, 0);

            return futureAQIO3Only;

        }

        public static FutureAQIs AQIForecastEquation(List<WeatherDataFromAPI> weatherTime, int index, double eightHourO3, double eightHourCO, double oneHourNO2)
        {
            // R^2 = 0.75
            double FutureO3AQI = (double)(5.3 * weatherTime[index].WindSpeed) + (double)(0.4 * weatherTime[index].TemperatureC) +
               (double)(0.1 * weatherTime[index].Humidity) + ((double)0.7 * eightHourO3);

            // R^2 = 0.48
            double FutureCOAQI = -(double)(0.03 * weatherTime[index].TemperatureC) +
                (double)(0.01 * weatherTime[index].Humidity) + ((double)0.6 * eightHourCO);

            // R^2 = 0.28 -- lowered Windspeed effect by tenfold and NO2 AQIs seem to come out more reasonable
            double FutureNO2AQI = 55.2 - (double)(1.75 * weatherTime[index].WindSpeed) -
                (double)(1.6 * weatherTime[index].TemperatureC) + ((double)0.4 * oneHourNO2);

            FutureAQIs futureAQI3Pollutants = new FutureAQIs((int)Math.Round(FutureO3AQI), (int)Math.Round(FutureCOAQI), (int)Math.Round(FutureNO2AQI));

            return futureAQI3Pollutants;

        }


    }
}